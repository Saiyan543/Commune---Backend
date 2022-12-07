using Main.Global.Library.RabbitMQ.Messages;
using Main.Global.Library.RabbitMQ.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace Main.Global.Library.RabbitMQ
{
    public interface IPublisher
    {
        void Publish(BaseMessage message);
    }

    public sealed class Publisher : IPublisher, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly Serilog.ILogger _log;
        private readonly RetryPolicy Policy;
        private readonly RabbitMqFactoryOptions _options;

        public Publisher(Serilog.ILogger log, IConfiguration configuration)
        {
            _log = log;
            _options = new RabbitMqFactoryOptions();
            configuration.Bind("RabbitMqOptions", _options);

            Policy = Polly.Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _log.Warning(ex, $"Could not connect to RabbitMQ",
                    $"{time.TotalSeconds:n1}", ex.Message);
                });

            var factory = new ConnectionFactory()
            {
                UserName = _options.Username,
                Password = _options.Password,
                HostName = _options.HostName,
                Port = _options.Port,
                VirtualHost = _options.VirtualHost,
            };

            Policy.Execute(() => _connection = factory.CreateConnection());

            _channel = _connection.CreateModel();

            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;
        }

        public void Publish(BaseMessage msg)
        {
            var message = JsonConvert.SerializeObject(msg);
            var body = Encoding.UTF8.GetBytes(message);

            Policy.Execute(() =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.DeliveryMode = 1;

                _channel.BasicPublish(
                    exchange: msg.ExchangeName,
                    routingKey: msg.RoutingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e) =>
                _log.Warning("A RabbitMQ connection is shutdown. Trying to re-connect...");

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e) =>
                _log.Warning("A RabbitMQ connection throw exception. Trying to re-connect...");

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason) =>
                _log.Information("A RabbitMQ connection is on shutdown.");

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                try
                {
                    _channel.Close();
                    _connection.Close();
                }
                catch (IOException ex)
                {
                    _log.Fatal(ex.ToString());
                }
            }
        }
    }
}