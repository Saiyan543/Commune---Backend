
using Main.Global.Helpers;
using Main.Global.Library.AutoMapper;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Main.Slices.Rota.Models.Base;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Models.Dtos.In;
using Main.Slices.Rota.Models.enums;
using Main.Slices.Rota.Services.Rota;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Neo4j.Driver;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Linq;

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

        private const string RotaPrefix = "rota";

        private static string CurrentDate = DateTime.UtcNow.ToShortDateString();
        private static string DateToDelete = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)).ToShortDateString();
        private static string DateToChange = DateTime.UtcNow.AddDays(6).ToShortDateString();
        private static string DateToCreate = DateTime.UtcNow.AddDays(13).ToShortDateString();

        // Models


        public async Task ManipulateSchedule(string id, UpcomingRota dto)
        {
    //       if(dto.Date < DateTime.UtcNow.AddDays(7))
               await _redis.HashSetAsync(RotaPrefix + id, dto.Date.ToShortDateString(), JsonConvert.SerializeObject(dto));

           // throw        
        }

        public async Task SecurityUpdateAttendance(string id, UpdatePersonelAttendanceDto model)
        {
            var shift = await _redis.StringGetAsync(id).ContinueWith(x => x.Result.RedisDeserialize<ShiftModel>());
            shift.Personel.Where(x => x.SecurityId.Equals(id)).First().Set((x) => x.Attendance = model.Attendance);
            await _redis.StringSetAsync(id, shift.Serialize());
            
        }


        
        public async Task ClubManipulateShifts(string id, DateTime date, Func<ShiftModel, ShiftModel> func)
        {
            //if (dto.End < DateTime.UtcNow.AddDays(7))
            //    throw new Exception();

            var key = await _redis.HashGetAsync(RotaPrefix + id, date.ToShortDateString());
            var model = await _redis.StringGetAsync(key.ToString()).ContinueWith(x => x.Result.RedisDeserialize<ShiftModel>());
            await _redis.StringSetAsync(key.ToString(), (func(model)).Serialize());
            
        }



        

        public async Task DeleteRota(string id) => await _redis.KeyDeleteAsync(RotaPrefix+id);
        public async Task InitialiseRota(string id)
        {

            //var rota = new HashEntry[7];
            //for (int i = 7, j = 0; i < 14; i++, j++)
            var rota = new HashEntry[14];
            for (int i = 0; i < 14; i++)
            {
                while (i < 7)
                {
                    rota[i] = new HashEntry(DateTime.UtcNow.AddDays(i - 1).ToShortDateString(), "key" + "NameofcluborSomething" + i.ToString());
                    var security = new List<Security>() { new Security("Marky", Guid.NewGuid().ToString(), Attendance.Confirmed), new Security("Jonny boi", Guid.NewGuid().ToString(), Attendance.Confirmed), new Security("belle thorn", Guid.NewGuid().ToString(), Attendance.Confirmed) };
                    _redis.StringSetAsync("key" + "NameofcluborSomething" + i.ToString(), JsonConvert.SerializeObject(
                        new ShiftModel("Prancing Pony", Guid.NewGuid().ToString(), DateTime.UtcNow.AddDays(i - 1), DateTime.UtcNow.AddDays(i - 1).AddHours(7), EventStatus.Ok, security.AsEnumerable())), TimeSpan.FromMinutes(2));
                    i++;
                }

                    
                        rota[i] = new HashEntry(DateTime.UtcNow.AddDays(i - 1).ToShortDateString(),
                            JsonConvert.SerializeObject(new SecurityUpcomingRota(null, null,DateTime.UtcNow.AddDays(i - 1))));

                    
                };

                await _redis.HashSetAsync(RotaPrefix + id, rota);
            


        }
        public async Task Increment(string id)
        {
            var key = RotaPrefix + id;

            await _redis.Write(
                  async (db) => await db.HashSetAsync(key, DateToChange, "key" + "NameofcluborSomething" + 6.ToString()),
                  async (db) => await db.HashDeleteAsync(key, DateToDelete),
                  async (db) => await db.HashSetAsync(key, DateToCreate, JsonConvert.SerializeObject(new SecurityUpcomingRota(null, null, DateTime.UtcNow.AddDays(6))))
                );



        }
        //public async Task Increment(string id, int n)//(string id)
        //{
        //    var key = RotaPrefix + id;

        //    await _redis.Write(
        //          //async (db) => await db.HashDeleteAsync(key, DateTime.UtcNow.ToShortDateString()),
        //          async (db) => await db.HashDeleteAsync(key, DateTime.UtcNow.AddDays(n).ToShortDateString()),
        //          //async (db) => await db.HashSetAsync(key, DateTime.UtcNow.AddDays(7).ToShortDateString(), JsonConvert.SerializeObject(new RotaModel(null, null)))
        //          async (db) => await db.HashSetAsync(key, DateTime.UtcNow.AddDays(7+n).ToShortDateString(), JsonConvert.SerializeObject(new RotaModel(null, null)))
        //        );



        //}



    }
}
