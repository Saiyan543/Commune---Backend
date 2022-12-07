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
        Task<IEnumerable<Shift>> GetClubShifts(string id);
        Task<IEnumerable<T>> GetSchedule<T>(string id);
        Task<IEnumerable<Shift>> GetSecurityShifts(string id);
        Task<T> GetUpComingRota<T>(string id, string date);
        Task InitialiseRota(string id, string role);
        Task ManipulateSchedule(string id, DateTime dtoDate, string serializedDto);
        Task SecurityUpdateAttendance(string clubId, UpdatePersonelAttendanceDto model);
        Task UpdateShiftDetails(string clubId, UpdateShiftDto dto);
        Task X();
    }
}
