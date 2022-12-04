using Homestead.Slices.Rota.Models.Db;
using Main.Slices.Rota.Services.Rota;
using Neo4j.Driver;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Homestead.Slices.Rota.Services.Rota
{
    public partial class RotaService : IRotaService
    {
        private readonly IDatabase _redis;
        private readonly IDriver _driver;

        public RotaService(IDatabase redis, IDriver driver)
        {
            _redis = redis;
            _driver = driver;
        }

        private const string ClubPrefix = "c";
        private const string SecurityPrefix = "s";
        private const string RotaPrefix = "r";



    
        
        public async Task DeleteRota(string id) => await _redis.HashDeleteAsync(RotaPrefix, id);
        public async Task InitialiseRota(string id)
        {

            var rota = new HashEntry[14];
            for(int i = 0; i < 14; i++)
            {
                rota[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), JsonConvert.SerializeObject(new SecurityRotaModel("name", id, null, null, false)));
            
            };

            await _redis.HashSetAsync(RotaPrefix+id, rota);

        }




        
    }
}
