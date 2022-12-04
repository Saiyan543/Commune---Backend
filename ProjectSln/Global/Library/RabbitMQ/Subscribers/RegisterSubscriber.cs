﻿using Dapper;
using Main.Global;
using Main.Global.Library.RabbitMQ;
using Main.Global.Library.RabbitMQ.Messages;
using Newtonsoft.Json;

namespace Main.Global.Library.RabbitMQ.Subscribers
{
    public sealed class RegisterSubscriber : SubscriberBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RegisterSubscriber(Serilog.ILogger log, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
            : base(log, configuration, new Options.RabbitMqQueueOptions(Constants.RegisterQueue, Constants.Homestead, Constants.Register)) => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ProcessEvent(string notificationMessage)
        {
            var content = JsonConvert.DeserializeObject<Register>(notificationMessage);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ProfileId", content.ProfileId, System.Data.DbType.String);
            parameters.Add("Username", content.UserName, System.Data.DbType.String);
            parameters.Add("Role", content.Role, System.Data.DbType.String);

            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var services = scope.ServiceProvider.GetRequiredService<IServiceManager>();
                await services.Profile.InitialiseProfile(parameters, parameters, parameters);
                await services.Contract.InitialiseNode(content.ProfileId, content.UserName, content.Role);
            }
        }
    }
}