using Dapper;
using Main.Global.Helpers;
using Main.Global.Library.RabbitMQ.Messages;

namespace Main.Global.Library.RabbitMQ.Subscribers
{
    public sealed class RegisterSubscriber : SubscriberBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RegisterSubscriber(Serilog.ILogger log, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
            : base(log, configuration, new Options.RabbitMqQueueOptions(Constants.RegisterQueue, Constants.Homestead, Constants.Register)) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ProcessEvent(string notificationMessage)
        {
            var content = notificationMessage.Deserialize<RegisterMessage>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ProfileId", content.ProfileId, System.Data.DbType.String);
            parameters.Add("Username", content.UserName, System.Data.DbType.String);
            parameters.Add("Role", content.Role, System.Data.DbType.String);

            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var services = scope.ServiceProvider.GetRequiredService<IServiceManager>();
                await services.Profile.InitialiseProfile(parameters, parameters, parameters);
                await services.Contract.InitialiseNode(content.ProfileId, content.UserName, content.Role);
                await services.Rota.InitialiseRota(content.ProfileId, content.Role);
            }
        }
    }
}