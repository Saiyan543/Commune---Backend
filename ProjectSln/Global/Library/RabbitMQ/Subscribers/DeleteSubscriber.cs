using Dapper;
using Main.Global.Library.RabbitMQ.Messages;
using Newtonsoft.Json;

namespace Main.Global.Library.RabbitMQ.Subscribers
{
    public sealed class DeleteSubscriber : SubscriberBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DeleteSubscriber(Serilog.ILogger log, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
            : base(log, configuration, new Options.RabbitMqQueueOptions(Constants.DeleteQueue, Constants.Homestead, Constants.Delete)) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ProcessEvent(string notificationMessage)
        {
            var content = JsonConvert.DeserializeObject<Delete>(notificationMessage);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ProfileId", content.ProfileId, System.Data.DbType.String);

            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var services = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                await services.Profile.DeleteProfile(parameters, parameters);

                await services.Contract.DeleteNode(content.ProfileId);
            }
        }
    }
}