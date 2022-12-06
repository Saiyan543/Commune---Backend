using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Main.Slices.Rota.Services.Rota;
using Neo4j.Driver;

namespace Homestead.Slices.Rota.Services.Rota
{

    public partial class RotaService : IRotaService
    {
        public async Task<IEnumerable<TRota>> GetSchedule<TRota>(string id)
        {
            var models = await _redis.HashGetAllAsync(RotaPrefix + id);
            
               return models.OrderBy(x => x.Name.ToString())
                .Skip(7).ResultOrEmpty()
                .Select(x => x.Value
                .RedisDeserialize<TRota>());
        }

        public async Task<IEnumerable<TShift>> GetShifts<TShift>(string id)
        {
            var resWithKeys = await _redis.HashGetAllAsync(RotaPrefix + id)
                .ContinueWith(x => x.Result.OrderBy(x => x.Name.ToString().Take(7)));

            List<TShift> shifts = new();
            foreach (var res in resWithKeys)
                shifts.Add(
                    await _redis.StringGetAsync(res.Value.ToString())
                    .ContinueWith(x => x.Result
                    .RedisDeserialize<TShift>()));

            return shifts;
        }







    }
}
