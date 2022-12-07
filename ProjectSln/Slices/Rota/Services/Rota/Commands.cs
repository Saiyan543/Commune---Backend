
using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Main.Slices.Rota.Models.Base;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Models.Dtos.In;
using Main.Slices.Rota.Models.enums;
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
        private readonly Serilog.ILogger _logger;

        public RotaService(Serilog.ILogger logger, IDatabase redis, IDriver driver)
        {
            _redis = redis;
            _driver = driver;
            _logger = logger;
        }

        private const string RotaPrefix = "rota";
        private const string ShiftPrefix = "shift";

        private static string CurrentDate = DateTime.UtcNow.ToShortDateString();
        private static string DateToDelete = DateTime.UtcNow.Subtract(TimeSpan.FromDays(0)).ToShortDateString();
        private static string DateToChange = DateTime.UtcNow.AddDays(7).ToShortDateString();
        private static string DateToCreate = DateTime.UtcNow.AddDays(14).ToShortDateString();
        private static int SkipTake = 7;
        private static string ProxySecurityId = "shfdowqeihfnoifheoipfej";
        private static string ProxyShiftId = "keyNameofcluborSomething";


        public async Task ManipulateSchedule(string id, DateTime dtoDate, string serializedDto)
        {
            await _redis.HashSetAsync(id, dtoDate.ToShortDateString(), serializedDto);
        }


        public async Task UpdateShiftDetails(string clubId, UpdateShiftDto dto)
        {
            await ManipulateShift(clubId, DateTime.Now, (shift) => { shift.Status = dto.EventStatus; return shift; });
        }

        public async Task SecurityUpdateAttendance(string clubId, UpdatePersonelAttendanceDto model)
        {
            await ManipulateShift(clubId, DateTime.Now, (shift) =>
            {
                var toUpdate = shift.Personel.ToList().Where(x => x.SecurityId.Equals(ProxySecurityId)).FirstOrDefault();
                var ss = shift.Personel.ToList().IndexOf(toUpdate);
                shift.Personel.RemoveAt(ss);
                shift.Personel.Add(new Personel(toUpdate.SecurityId, toUpdate.SecurityName, model.AttendanceOut()));
                return shift;
            });
        }

        Func<IDatabase, string, string, Shift, Task> setShiftHash = async (db, s, d, m) =>
        {
            await db.HashSetAsync(s, d, m.Serialize());
        };



        
        public async Task ManipulateShift(string id, DateTime date, Func<Shift, Shift> func)
        {
            var shift = await getShiftHash(_redis, id, date.ToShortDateString());
            await setShiftHash(_redis, id, date.ToShortDateString(), func(shift));

        }





        public async Task InitialiseRota(string id, string role)
        {

            var isClub = role.Equals("Club");
            var hash = new HashEntry[14];

            if (!isClub)
            {
                for (int i = 0; i < 7; i++)
                {
                    //  would otherwise contain a the Id of the(clubId +Date) to retrieve the shift model
                    hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), "void");

                }
                for (int i = 7; i < 14; i++)
                {
                    hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), new InitSecurityUp().Serialize());
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), new NullShift().Serialize());
                }

                for (int i = 7; i< 14; i++)
                {
                    hash[i] = new HashEntry(DateTime.UtcNow.AddDays(i).ToShortDateString(), new InitClubUp().Serialize());
                }
            }

            await _redis.HashSetAsync(id, hash);

        }


        public async Task DeleteRota(string id) => await _redis.KeyDeleteAsync(id);


    }
}






