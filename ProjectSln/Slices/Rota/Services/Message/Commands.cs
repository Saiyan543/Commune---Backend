
using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Neo4J.Extensions;
using Main.Slices.Rota.Models.Dtos.In;
using Main.Slices.Rota.Services.Message;
using Microsoft.Extensions.Caching.Distributed;
using Neo4j.Driver;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Homestead.Slices.Rota.Services.Contract
{
    public partial class MessageService : IMessageService
    {
        private readonly IDatabase _redis;
        private readonly IDriver _driver;
        private readonly Serilog.ILogger _logger;

        public MessageService(Serilog.ILogger logger, IDatabase redis, IDriver driver)
        {
            _redis = redis;
            _driver = driver;
            _logger = logger;
        }

        private const string MessagePrefix = "message";

        public async Task AppendMessageThread(string actorId, string targetId, MessageDto message)
        {
            string key = Keys.OrdinalKey(actorId, targetId, MessagePrefix);
            if (await _redis
            .KeyExistsAsync(key))
                await _redis.ListRightPushAsync(key, message.Serialize());
        }

        public async Task SendMessageRequest(string actorId, string targetId, string message)
        {
        
            await _driver.RunAsync(
                @"MATCH (a:User{id:$actorId}) MATCH (b:User{id:$targetId}) MERGE (a)-[:Thread {status:'pending'}]-(b) RETURN a.name, b.name",
                new { actorId = actorId, targetId = targetId });

            var key = Keys.OrdinalKey(actorId, targetId, MessagePrefix);

            if (!await _redis
                .KeyExistsAsync(key))
                await _redis.ListRightPushAsync(key, new MessageDto(actorId, message, DateTime.UtcNow).Serialize());
        }

        public async Task RespondToMessageRequest(string actorId, string targetId, MessageRequestResponse response)
        {
            await _driver.RunAsync(@"MATCH (a:User{id:$actorId})-[t:Thread]-(b:User{id:$targetId}) SET t.status = $response ",
                new { response = response.Response, actorId = actorId, targetId = targetId });

            await AppendMessageThread(actorId, targetId, new MessageDto(actorId, response.Message, DateTime.UtcNow));
        }

        public async Task DeleteThread(string actorId, string targetId)
        {
            await _redis.KeyDeleteAsync(Keys.OrdinalKey(MessagePrefix, actorId, targetId));

            await _driver.RunAsync(@"MATCH (n:User {id: $actorId})-[t:Thread{status:'Accepted'}]->(m:User{id:$targetId}) DETACH DELETE t",
                new { actorId = actorId, targetId = targetId });
        }
    }
}