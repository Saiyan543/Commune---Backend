using Main.Global.Library.RabbitMQ.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace Main.Global.Library.RabbitMQ.Subscribers
{
    public abstract class SubscriberBase : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly Serilog.ILogger _log;
        private readonly RetryPolicy Policy;
        private readonly RabbitMqFactoryOptions _options;
        private readonly RabbitMqQueueOptions _queue;

        public SubscriberBase(Serilog.ILogger log, IConfiguration configuration, RabbitMqQueueOptions queue)
        {
            _log = log;
            _queue = queue;
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

            _channel.ExchangeDeclare(
                exchange: queue.exchangeName,
                type: "direct",
                durable: true,
                autoDelete: false,
                arguments: null);

            _channel.QueueDeclare(
                queue: queue.queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                queue: queue.queueName,
                exchange: queue.exchangeName,
                routingKey: queue.routingKey);

            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ModuleHandle, ea) =>
            {
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                await ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queue.queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        protected virtual async Task ProcessEvent(string notificationMessage)
        { }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e) =>
           _log.Warning("A RabbitMQ connection is shutdown. Trying to re-connect...");

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e) =>
             _log.Warning("A RabbitMQ connection throw exception. Trying to re-connect...");

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason) =>
             _log.Information("A RabbitMQ connection is on shutdown.");
    }
}