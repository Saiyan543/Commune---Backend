using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Neo4J.Extensions;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Models.Dtos.In;
using Main.Slices.Rota.Models.Dtos.Out;
using Main.Slices.Rota.Services.Message;
using Microsoft.IdentityModel.Tokens;
using Neo4j.Driver;
using Newtonsoft.Json;

namespace Homestead.Slices.Rota.Services.Contract
{
    public partial class MessageService : IMessageService
    {
        public async Task<IEnumerable<MessageDto>?> RetrieveMessageRequests(string id)
        {
            var requests = await _driver.ReadAsync(@"MATCH (a:User{id:$id})-[r:Thread{status:'pending'}]-(b:User) RETURN r.message, b.name", new { id = id },
                    (results) => (results["r.message"].As<string>(), results["b.name"].As<string>()));
            return requests
                .ResultOrEmpty()
                .Transfer((result) => new MessageDto(result.Item1, result.Item2, DateTime.UtcNow));
        }

        private const string CachedMessageLocationsPrefix = "messageLocations";


        private async Task<IEnumerable<Id_Name>> CachedMessageLocationsAndNames(string id)
        {
            var result = await _redis.GetRecordsAsync<Id_Name>(CachedMessageLocationsPrefix + id);
            if (!result.IsNullOrEmpty())
                return result;
            
            var locations = await _driver
            .ReadAsync(@"MATCH (n:User {id: $Id})-[:Thread{status:'Accepted'}]-(m:User) RETURN m.id, m.name", new { Id = id },
                (results) => new Id_Name(results["m.id"].As<string>(), results["m.name"].As<string>()));
            
            await _redis.SetRecordAsync<IEnumerable<Id_Name>>(id + CachedMessageLocationsPrefix, locations, TimeSpan.FromMinutes(5));
            return locations.ResultOrEmpty();
        }
        
        public async Task<IEnumerable<MessageThreadDto>> RetrieveThreads(string id)
        {
            var locations = await CachedMessageLocationsAndNames(id);
            var messageLists = locations
                .Select(x => _redis.ListRange(Keys.OrdinalKey(x.Id, id, MessagePrefix), 0, -1));

            List<MessageThreadDto> list = new();
            for (int i = 0; i < locations.Count(); i++)
            {
                list.Add(
                    new MessageThreadDto(
                        messageLists.ElementAt(i).Select(x => JsonConvert.DeserializeObject<MessageDto>(x)),
                        locations.ElementAt(i).Id,
                        locations.ElementAt(i).Name));
            }
            return list;
        }
    }
}