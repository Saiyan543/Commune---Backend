using Main.DataAccessConfig.Redis.Extensions;
using Main.Global.Helpers;
using Main.Slices.Messages.Models;
using StackExchange.Redis;

namespace Main.Slices.Messages
{
    public interface IMessageService
    {
        Task AcceptMessageRequest(string actorId, string actorName, string targetId, string targetName, bool accept);
        Task DeleteKey(string actorId, string targetId);
        Task<IEnumerable<MessageDto>> RetrieveMessageRequests(string id);
        Task<IEnumerable<MessageThreadDto>> RetrieveMessageThreads(string id);
        Task SendMessage(string actorId, string targetId, MessageDto dto);
        Task SendMessageRequest(string actorId, string targetId, MessageRequestDto dto);
    }


    public class MessageService : IMessageService
    {
        private readonly IDatabase _redis;

        public MessageService(Serilog.ILogger logger, IDatabase redis)
        {
            _redis = redis;
        }
        private const string RequestPrefix = "Request";
        public async Task SendMessageRequest(string actorId, string targetId, MessageRequestDto dto)
        {
            if (!await _redis.KeyExistsAsync(Keys.OrdinalKey(actorId, targetId))){
                await _redis.HashSetAsync(RequestPrefix+targetId, actorId, dto.Serialize(), When.NotExists);
            }
        }


        public async Task AcceptMessageRequest(string actorId, string actorName, string targetId, string targetName, bool accept)
        {

            if (accept)
            {
                var requestMessage = await _redis.HashGetAsync(RequestPrefix + actorId, targetId);
                var request = requestMessage.Deserialize<MessageRequestDto>();
                await _redis.Write(
                     (db) => db.HashDeleteAsync(RequestPrefix + actorId, targetId),
                     (db) => db.ListLeftPushAsync(Keys.OrdinalKey(actorId, targetId), new MessageDto(request.Date, request.SenderId, request.Body).Serialize()),
                     (db) => db.HashSetAsync(actorId, Keys.OrdinalKey(actorId, targetId), targetName),
                     (db) => db.HashSetAsync(targetId, Keys.OrdinalKey(actorId, targetId), actorName));
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
            return all.Select(x => x.Value.Deserialize<MessageRequestDto>());
        }



        public async Task<IEnumerable<MessageThreadDto>> RetrieveMessageThreads(string id)
        {
            var Ids_Names = await _redis.HashGetAllAsync(id);
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
            var exists = await _redis.HashExistsAsync(targetId, Keys.OrdinalKey(actorId, targetId));
            if (exists)
            { await _redis.HashDeleteAsync(actorId, Keys.OrdinalKey(actorId, targetId)); }
            else
            {
                await _redis.Write(
                    (db) => db.HashDeleteAsync(actorId, Keys.OrdinalKey(actorId, targetId)),
                    (db) => db.KeyDeleteAsync(Keys.OrdinalKey(actorId, targetId)));
            }
        }


        public async Task DeleteUser(string id)
        {
            
        }

    }
}
