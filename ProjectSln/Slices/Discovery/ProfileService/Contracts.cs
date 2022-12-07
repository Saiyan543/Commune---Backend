using Dapper;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Library.ApiController.Responses;
using Main.Slices.Discovery.Models.Dtos;


namespace Main.Slices.Discovery.ProfileService
{
    public interface IProfileService
    {
        Task DeleteProfile(params DynamicParameters[] obj);

        Task InitialiseProfile(params DynamicParameters[] obj);

        Task<BaseResponse> GetCoordinates(string id);

        Task<BaseResponse> GetProfile(string id);

        Task<IEnumerable<ProfileView>> Search(double range, string sql, Coordinate coordinateFrom);

        Task UpdateDays(string id, DaysDto dto);

        Task UpdateLocation(string id, Coordinate dto);

        Task UpdateProfile(string id, ProfileDto dto);
    }
}