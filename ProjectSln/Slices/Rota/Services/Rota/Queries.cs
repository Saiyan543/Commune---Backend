using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Services.Rota;
using Neo4j.Driver;
using StackExchange.Redis;

namespace Homestead.Slices.Rota.Services.Rota
{

    public partial class RotaService : IRotaService
    {

        Func<IDatabase, string, string, Task<Shift>> getShiftHash = async (db, s, d) =>
        {
            var hash = await db.HashGetAsync(s, d);
            return hash.Deserialize<Shift>();
        };





        public async Task<T> GetUpComingRota<T>(string id, string date)
        {
            var hash = await _redis.HashGetAsync(id, date);
            return hash.Deserialize<T>();
        }




        public async Task<IEnumerable<T>> GetSchedule<T>(string id) =>
            await _redis.HashGetAllAsync(id)
                .ContinueWith(x => x.Result
                .OrderBy(x => x.Name).Skip(SkipTake)).ContinueWith(x => x.Result
                .Select(x => x.Value.Deserialize<T>()));


        public async Task<IEnumerable<Shift>> GetClubShifts(string id)
        {
            var s  =await _redis.HashGetAllAsync(id)
                .ContinueWith(x => x.Result
                .OrderBy(x => x.Name).Take(SkipTake)).ContinueWith(x => x.Result
                .Select(x => x.Value.Deserialize<Shift>()));

            return s;
        }

            public async Task<IEnumerable<Shift>> GetSecurityShifts(string id)
        {
            var shiftsHashes = await _redis.HashGetAllAsync(id)
                .ContinueWith(x => x.Result
                .OrderBy(x => x.Name).Take(SkipTake));

            List<Shift> shifts = new();
            for (int i = 0; i < shiftsHashes.Count(); i++)
            {
                if (shiftsHashes.ElementAt(i).Value.Equals("void"))
                { shifts.Add(new NullShift()); continue; }
                
                shifts.Add(
                    await _redis.HashGetAsync(shiftsHashes.ElementAt(i).Value.ToString(), shiftsHashes.ElementAt(i).Name.ToString())
                    .ContinueWith(x => x.Result
                    .Deserialize<Shift>()));
            }
            return shifts;
 
        }






        




    }
}
