using Main.DataAccessConfig.Redis.Extensions;
using Main.Global.Helpers;
using Main.Slices.Messages.Models;
using StackExchange.Redis;

namespace Main.Slices.Messages
{
    public interface IMessageService
    {
        Task AcceptMessageRequest(string actorId, string targetId, bool accept);
        Task DeleteKey(string actorId, string targetId);
        Task DeleteUser(string id);
        Task Initialise(string userId, string username);
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
        private const string RequestPrefix = "Requests";
        private const string ThreadPrefix = "Threads";
        private const string NamePrefix = "Name";
        public async Task SendMessageRequest(string actorId, string targetId, MessageDto dto)
        {
            if (!await _redis.KeyExistsAsync(Keys.OrdinalKey(actorId, targetId)) && await _redis.KeyExistsAsync(NamePrefix+targetId)){
                await _redis.HashSetAsync(RequestPrefix+targetId, actorId, dto.Serialize(), When.NotExists);
            }
        }


        public async Task AcceptMessageRequest(string actorId, string targetId, bool accept)
        {          
            if (accept && await _redis.KeyExistsAsync(NamePrefix + targetId) && await _redis.KeyExistsAsync(NamePrefix + actorId))
            {
                var actorName = await _redis.StringGetAsync(NamePrefix + actorId);
                var targetName = await _redis.StringGetAsync(NamePrefix + targetId);
                
                var requestMessage = await _redis.HashGetAsync(RequestPrefix + actorId, targetId);

                await _redis.Write(
                     (db) => db.HashDeleteAsync(RequestPrefix + actorId, targetId),
                     (db) => db.ListLeftPushAsync(Keys.OrdinalKey(actorId, targetId), requestMessage),
                     (db) => db.HashSetAsync(ThreadPrefix+actorId, Keys.OrdinalKey(actorId, targetId), targetName),
                     (db) => db.HashSetAsync(ThreadPrefix+targetId, Keys.OrdinalKey(actorId, targetId), actorName));
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
            var Ids_Names = await _redis.HashGetAllAsync(ThreadPrefix+id);
            List<MessageThreadDto> threads = new();
            foreach (var IdN in Ids_Names) // Name is Id, the hash value is the user name
            {
                var messages = await _redis.ListRangeAsync(IdN.Name.ToString(), 0, -1).ContinueWith(x => x.Result.Select(x => x.Deserialize<MessageDto>()));
                threads.Add(new MessageThreadDto(IdN.Value, messages)); 
            }
            return threads;
        }


        
        public async Task DeleteKey(string actorId, string targetId)
        {
            var ordinalKey = Keys.OrdinalKey(actorId, targetId);
            
            var exists = await _redis.HashExistsAsync(ThreadPrefix + targetId, ordinalKey);
            if (exists)
            {
                await _redis.HashDeleteAsync(ThreadPrefix+actorId, ordinalKey);
            }
            else
            {
                await _redis.Write(
                    (db) => db.HashDeleteAsync(ThreadPrefix+actorId, ordinalKey),
                    (db) => db.KeyDeleteAsync(ordinalKey));
            }
        }

        public async Task Initialise(string userId, string username)
        {
            await _redis.StringSetAsync(NamePrefix+userId, username);

        }
        public async Task DeleteUser(string id)
        {
            var Ids_Names = await _redis.HashGetAllAsync(ThreadPrefix + id);
           
            foreach (var idN in Ids_Names)
            {
               await DeleteKey(id, idN.Name.ToString().Replace(id, ""));
            }
            await _redis.Write(
                (db) => db.KeyDeleteAsync(RequestPrefix + id),
                (db) => db.KeyDeleteAsync(ThreadPrefix + id),
                (db) => db.KeyDeleteAsync(NamePrefix + id));
        }

    }
}
