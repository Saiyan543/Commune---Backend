using Dapper;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Library.ApiController.Responses;
using Main.Slices.Discovery.Models.Dtos.In;
using Main.Slices.Discovery.Models.Dtos.Out;

namespace Main.Slices.Discovery.ProfileService
{
    public interface IProfileService
    {
        Task DeleteProfile(params DynamicParameters[] obj);

        Task InitialiseProfile(params DynamicParameters[] obj);

        Task<BaseResponse> GetCoordinates(string id);

        Task<BaseResponse> GetProfile(string id);

        Task<IEnumerable<SearchProfileDto>> Search(double range, string sql, Coordinate coordinateFrom);

        Task UpdateDays(string id, DaysForManipulationDto inDto);

        Task UpdateLocation(string id, Coordinate inDto);

        Task UpdateProfile(string id, ProfileForManipulationDto inDto);
    }
}