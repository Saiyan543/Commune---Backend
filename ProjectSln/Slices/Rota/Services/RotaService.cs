using Main.DataAccessConfig.Redis.Extensions;
using Main.Global.Helpers;
using Main.Global.Library.ApiController.Responses;
using Main.Slices.Rota.Models.Rota;
using Neo4j.Driver;
using StackExchange.Redis;

namespace Main.Slices.Rota.Services
{
    public interface IRotaService
    {
        Task DeleteRota(string id);

        Task<IEnumerable<Shift>> GetClubShifts(string id);

        Task<IEnumerable<T>> GetSchedule<T>(string id);

        Task<IEnumerable<Shift>> GetSecurityShifts(string id);

        Task InitialiseRota(string id, string role);

        Task ManipulateSchedule(string id, DateTime day, string serializedDto);

        Task UpdateAttendance(string clubId, UpdateAttendanceDto model);

        Task DeleteShift(string clubId, DateTime date);
    }

    public class RotaService : IRotaService
    {
        private readonly IDatabase _redis;
        private readonly IDriver _driver;
        private readonly Serilog.ILogger _logger;

        public RotaService(Serilog.ILogger logger, IDatabase redis, IDriver driver)
        {
            _redis = redis;
            _driver = driver;
            _logger = logger;
        }

        public async Task ManipulateSchedule(string id, DateTime day, string serializedDto)
        {
            
            await _redis.HashSetAsync(id, day.ToShortDateString(), serializedDto);
           
        }
         
        
        public async Task DeleteShift(string clubId, DateTime date)
        {
            await _redis.HashSetAsync(clubId, date.ToShortDateString(), new NullShift().Serialize());
        }

        public async Task UpdateAttendance(string id, UpdateAttendanceDto dto)
        {
            var shift = await _redis.HashGetAsync(id, dto.Date.ToShortDateString())
                .ContinueWith(x => x.Result
                .Deserialize<Shift>());

            var toUpdate = shift.Personel
                .ToList()
                .Where(x => x.SecurityId.Equals(id))
                .FirstOrDefault();
            ;
            shift.Personel
                .RemoveAt(shift.Personel
                .ToList()
                .IndexOf(toUpdate));
            shift.Personel
                .Add(new Personel(toUpdate.SecurityId, toUpdate.SecurityName, dto.Attendance));
        }

        public async Task<IEnumerable<T>> GetSchedule<T>(string id) =>
            await _redis.HashGetAllAsync(id)
                .ContinueWith(x => x.Result
                .OrderBy(x => x.Name).Skip(7)).ContinueWith(x => x.Result
                .Select(x => x.Value.Deserialize<T>()));

        public async Task<IEnumerable<Shift>> GetClubShifts(string id) =>
            await _redis.HashGetAllAsync(id)
                .ContinueWith(x => x.Result
                .OrderBy(x => x.Name).Take(7)).ContinueWith(x => x.Result
                .Select(x => x.Value.Deserialize<Shift>()));

        public async Task<IEnumerable<Shift>> GetSecurityShifts(string id)
        {
            var shiftsHashes = await _redis.HashGetAllAsync(id)
                .ContinueWith(x => x.Result
                .OrderBy(x => x.Name).Take(7));

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

        public async Task DeleteRota(string id) => await _redis.KeyDeleteAsync(id);

        public async Task InitialiseRota(string id, string role)
        {
            var hash = new HashEntry[14];

            if (role.Equals("Club"))
            {
                for (int i = 0; i < 7; i++) { hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), "void"); }
                for (int i = 7; i < 14; i++) { hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), new Schedule_Security(null, null).Serialize()); }

                await _redis.HashSetAsync(id, hash);
            }
            else if (role.Equals("Security"))
            {
                for (int i = 0; i < 7; i++) { hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), new NullShift().Serialize()); }
                for (int i = 7; i < 14; i++) { hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), new Schedule_Club(null, null, default).Serialize()); }

                await _redis.HashSetAsync(id, hash);
            }
        }
    }
}