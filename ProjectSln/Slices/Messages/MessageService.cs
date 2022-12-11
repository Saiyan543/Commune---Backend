using Main.Global.Helpers;
using Main.Redis.Extensions;
using Main.Slices.Messages.Messages;
using Main.Slices.Messages.Models;
using StackExchange.Redis;

namespace Main.Slices.Messages
{
    public interface IMessageService
    {
        Task AcceptMessageRequest(string actorId, string targetId, bool accept);
        Task DeleteKey(string actorId, string targetId);
        Task<IEnumerable<MessageDto>> RetrieveMessageRequests(string id);
        Task<IEnumerable<MessageThreadDto>> RetrieveMessageThreads(string id);
        Task SendMessage(string actorId, string targetId, MessageDto dto);
        Task SendMessageRequest(string actorId, string targetId, MessageDto dto);
    }


    public class MessageService : IMessageService
    {
        private readonly IDatabase _redis;

        public MessageService(Serilog.ILogger logger, IDatabase redis)
        {
            _redis = redis;
        }
        private const string RequestPrefix = "Request";
        public async Task SendMessageRequest(string actorId, string targetId, MessageDto dto)
        {
            if (!await _redis.KeyExistsAsync(Keys.OrdinalKey(actorId, targetId))){
                await _redis.HashSetAsync(RequestPrefix+targetId, actorId, dto.Serialize(), When.NotExists);
            }
        }


        public async Task AcceptMessageRequest(string actorId, string targetId, bool accept)
        {

            if (accept)
            {
                var requestMessage = await _redis.HashGetAsync(RequestPrefix + actorId, targetId);
                await _redis.Write(
                    async (db) => db.HashDeleteAsync(RequestPrefix + targetId, actorId),
                    async (db) => db.ListLeftPushAsync(Keys.OrdinalKey(actorId, targetId), requestMessage),
                    async (db) => db.SetAddAsync(targetId, actorId),
                    async (db) => db.SetAddAsync(actorId, targetId));
            }
            else
            {
                await _redis.HashDeleteAsync(RequestPrefix + actorId, targetId);
            }

        }

        public async Task SendMessage(string actorId, string targetId, MessageDto dto)
        {
            if (await _redis.KeyExistsAsync(Keys.OrdinalKey(actorId, targetId))){
                await _redis.ListLeftPushAsync(Keys.OrdinalKey(actorId, targetId), dto.Serialize());
            }
        }

        public async Task<IEnumerable<MessageDto>> RetrieveMessageRequests(string id)
        {
            var all = await _redis.HashGetAllAsync(RequestPrefix+id);
            return all.Select(x => x.Value.Deserialize<MessageDto>());
        }


  
        public async Task<IEnumerable<MessageThreadDto>> RetrieveMessageThreads(string id)
        {
            var all = await _redis.SetMembersAsync(id);
            List<RedisValue[]> threads = new();
            foreach(var a in all)
            { threads.Add(await _redis.ListRangeAsync(Keys.OrdinalKey(id,a), 0, -1));}

            return threads.Select(x => new MessageThreadDto(x.Deserialize<MessageDto>()));
        }


        public async Task DeleteKey(string actorId, string targetId)
        {
            var exists = await _redis.SetMembersAsync(targetId);
            if (exists.Contains(actorId))
            { await _redis.SetRemoveAsync(actorId, targetId); }
            else
            {
                await _redis.Write(
                    (db) => db.SetRemoveAsync(actorId, targetId),
                    (db) => db.KeyDeleteAsync(Keys.OrdinalKey(actorId, targetId)));
            }
        }

    }
}
