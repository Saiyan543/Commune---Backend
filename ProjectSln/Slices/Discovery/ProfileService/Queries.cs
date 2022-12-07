using Dapper;
using Main.Global.Helpers;
using Main.Global.Helpers.Location;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Library.ApiController.Responses;
using Main.Global.Library.AutoMapper;
using Main.Slices.Discovery.DapperOrm.Extensions;
using Main.Slices.Discovery.Models.Dtos;
using Main.Slices.Discovery.Models.Responses;
using Main.Slices.Discovery.ProfileService;

namespace Homestead.Slices.Discovery.ProfileService
{
    public partial class ProfileService : IProfileService
    {
        public async Task<BaseResponse> GetCoordinates(string id)
        {
            var userCoordinates = await _context
             .ReadSingleAsync(async (c) => await c.QueryFirstOrDefaultAsync<Coordinate>(
                 @"SELECT Latitude, Longitude
                FROM Location
                WHERE ProfileId = @Id", new { Id = id }));
            if (userCoordinates is null)
                return new ProfileNotFoundResponse(id);
            return new Response<Coordinate>(userCoordinates);
        }
        
        public async Task<IEnumerable<ProfileView>> Search(double range, string sql, Coordinate coordinateFrom)
        {
            var profiles = await _context.ReadAsync(async (c) => await c.QueryAsync<ProfileDb>(sql));

            return profiles.ResultOrEmpty().Where(x => GeoLocation.IsInRange(range, coordinateFrom, new Coordinate(x.Longitude, x.Longitude)))
                .Map<ProfileDb, ProfileView>();
        }

        public async Task<BaseResponse> GetProfile(string id)
        {
            var result = await _context
            .ReadSingleAsync(async (c) => await c.QueryFirstOrDefaultAsync<ProfileDb>(
                @"SELECT *
                FROM Profile AS p
                LEFT JOIN Days as d on d.ProfileId = p.Id
                LEFT JOIN Location as l on l.ProfileId = p.Id
                WHERE p.Id = @Id", new { Id = id }));

            if (result is null)
                return new ProfileNotFoundResponse(id);

            var profile = result.Map<ProfileDb, ProfileView>();
            profile.PostCode = GeoLocation.GetPostCodeFromCoordinate(new Coordinate(result.Latitute, result.Longitude));

            return new Response<ProfileView>(profile);
        }
    }
}