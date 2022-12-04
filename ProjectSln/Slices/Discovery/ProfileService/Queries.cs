using Dapper;
using Homestead.Global.Library.ApiController.Responses;
using Main.Global.Helpers;
using Main.Global.Helpers.Location;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Library.ApiController.Responses;
using Main.Global.Library.AutoMapper;
using Main.Slices.Discovery.DapperOrm.Extensions;
using Main.Slices.Discovery.Models.Dtos.Db;
using Main.Slices.Discovery.Models.Dtos.Out;
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
                return new NotFoundResponse();
            return new Response<Coordinate>(userCoordinates);
        }

        public async Task<IEnumerable<SearchProfileDto>> Search(double range, string sql, Coordinate coordinateFrom)
        {
            var profiles = await _context.ReadAsync(async (c) => await c.QueryAsync<ProfileDbModel>(sql));

            return profiles
            .ResultOrEmpty()
            .Next((res) => res.Where(x => GeoLocation.IsInRange(range, coordinateFrom, new Coordinate(x.Longitude, x.Longitude))))
            .Map<ProfileDbModel, SearchProfileDto>();
        }

        public async Task<BaseResponse> GetProfile(string id)
        {
            var result = await _context
            .ReadSingleAsync(async (c) => await c.QueryFirstOrDefaultAsync<ProfileDbModel>(
                @"SELECT *
                FROM Profile AS p
                LEFT JOIN Days as d on d.ProfileId = p.Id
                LEFT JOIN Location as l on l.ProfileId = p.Id
                WHERE p.Id = @Id", new { Id = id }));

            if (result is null)
                return new NotFoundResponse();

            var profile = _mapper.Map<ProfileDto>(result);
            profile.PostCode = GeoLocation.GetPostCodeFromCoordinate(new Coordinate(result.Latitude, result.Longitude));

            return new Response<ProfileDto>(profile);
        }
    }
}