using Homestead.Slices.Rota.Services.Rota;
using Main.Slices.Rota.Models.Base;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Models.Dtos.In;
using Neo4j.Driver;
using StackExchange.Redis;
using static Homestead.Slices.Rota.Services.Rota.RotaService;

namespace Main.Slices.Rota.Services.Rota
{
    public interface IRotaService
    {
        Task DeleteRota(string id);
        Task<IEnumerable<TRota>> GetSchedule<TRota>(string id);
        Task<IEnumerable<TShift>> GetShifts<TShift>(string id);
        Task Increment(string id);
        Task InitialiseRota(string id);
        Task ManipulateSchedule(string id, UpcomingRota dto);
        Task ClubManipulateShifts(string id, DateTime dto, Func<ShiftModel, ShiftModel> func);
        Task SecurityUpdateAttendance(string id, UpdatePersonelAttendanceDto model);
    }
}
