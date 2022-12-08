using Main.Global.Helpers;
using Main.Slices.Rota.Models.Messages;
using Main.Slices.Rota.Neo4J.Extensions;
using Main.Slices.Rota.Redis.Extensions;
using Neo4j.Driver;
using StackExchange.Redis;

namespace Main.Slices.Rota.Services
{
    public interface IMessageService
    {
        Task AppendMessageThread(string actorId, string targetId, MessageDto message);

        Task DeleteThread(string actorId, string targetId);

        Task RespondToMessageRequest(string actorId, string targetId, MessageRequestResponseDto response);

        Task<IEnumerable<MessageDto>?> RetrieveMessageRequests(string id);

        Task<IEnumerable<MessageThreadDto>> RetrieveThreads(string id);

        Task SendMessageRequest(string actorId, string targetId, string message);
    }

    public sealed class MessageService : IMessageService
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

        public async Task<IEnumerable<MessageDto>?> RetrieveMessageRequests(string id)
        {
            var requests = await _driver.ReadAsync(@"MATCH (a:User{id:$id})-[r:Thread{status:'pending'}]-(b:User) RETURN r.message, b.name", new { id },
                    (results) => (results["r.message"].As<string>(), results["b.name"].As<string>()));
            return requests
                .ResultOrEmpty()
                .Transfer((result) => new MessageDto(result.Item1, result.Item2, DateTime.UtcNow));
        }

        public async Task<IEnumerable<MessageThreadDto>> RetrieveThreads(string id)
        {
            var locations = await _driver
            .ReadAsync<(string, string)>(@"MATCH (n:User {id: $Id})-[:Thread{status:'Accepted'}]-(m:User) RETURN m.id, m.name", new { Id = id },
              (results) => new(results["m.id"].As<string>(), results["m.name"].As<string>()));

            var messageLists = locations
                .Select(x => _redis.ListRange(Keys.OrdinalKey(x.Item1, id, MessagePrefix), 0, -1));

            List<MessageThreadDto> list = new();
            for (int i = 0; i < locations.Count(); i++)
            {
                list.Add(
                    new MessageThreadDto(
                        messageLists.ElementAt(i).Select(x => x.Deserialize<MessageDto>()),
                        locations.ElementAt(i).Item1,
                        locations.ElementAt(i).Item2));
            }
            return list;
        }

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
                new { actorId, targetId });

            var key = Keys.OrdinalKey(actorId, targetId, MessagePrefix);

            if (!await _redis
                .KeyExistsAsync(key))
                await _redis.ListRightPushAsync(key, new MessageDto(actorId, message, DateTime.UtcNow).Serialize());
        }

        public async Task RespondToMessageRequest(string actorId, string targetId, MessageRequestResponseDto response)
        {
            await _driver.RunAsync(@"MATCH (a:User{id:$actorId})-[t:Thread]-(b:User{id:$targetId}) SET t.status = $response ",
                new { response = response.Response, actorId, targetId });

            await AppendMessageThread(actorId, targetId, new MessageDto(actorId, response.Message, DateTime.UtcNow));
        }

        public async Task DeleteThread(string actorId, string targetId)
        {
            await _redis.KeyDeleteAsync(Keys.OrdinalKey(MessagePrefix, actorId, targetId));

            await _driver.RunAsync(@"MATCH (n:User {id: $actorId})-[t:Thread{status:'Accepted'}]->(m:User{id:$targetId}) DETACH DELETE t",
                new { actorId, targetId });
        }
    }
}