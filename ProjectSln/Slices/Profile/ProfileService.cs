using Dapper;
using Main.Global.Helpers;
using Main.Global.Helpers.Location;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Library.ApiController.Responses;
using Main.Global.Library.AutoMapper;
using Main.Slices.Discovery.DapperOrm.Context;
using Main.Slices.Discovery.DapperOrm.Extensions;
using Main.Slices.Discovery.Models.Dtos;
using Main.Slices.Discovery.Models.Responses;
using System.Data;

namespace Main.Slices.Profile
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

    public sealed class ProfileService : IProfileService
    {
        private readonly DapperContext _context;
        private readonly Serilog.ILogger _logger;

        public ProfileService(Serilog.ILogger logger, DapperContext context)
        {
            _context = context;
            _logger = logger;
        }

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
            var postCode = new PostcodeDto(GeoLocation.GetPostCodeFromCoordinate(new Coordinate(result.Latitute, result.Longitude)));

            return new Response<(ProfileView, PostcodeDto)>((profile, postCode));
        }

        public async Task UpdateLocation(string id, Coordinate coordinate) =>
            await _context.RunAsync(
                @"Update Location
                SET Latitude =  @Latitude, Longitude = @Longitude
                WHERE ProfileId = @ProfileId", new DynamicParameters().Fill(
                    ("Latitude", coordinate.Latitude, DbType.Double),
                    ("Longitude", coordinate.Longitude, DbType.Double),
                    ("ProfileId", id, DbType.String)));

        public async Task UpdateProfile(string id, ProfileDto dto) =>
            await _context.RunAsync(
                @"UPDATE Profile
                SET ActivelyLooking = @ActivelyLooking, ShowInSearch = @ShowInSearch, Bio = @Bio
                WHERE Id = @ProfileId",
                new DynamicParameters().Fill(
                    ("ProfileId", id, DbType.String),
                    ("Bio", dto.Bio, DbType.String),
                    ("ShowInSearch", dto.ShowInSearch, DbType.Boolean),
                    ("ActivelyLooking", dto.ActivelyLooking, DbType.Boolean)));

        public async Task UpdateDays(string id, DaysDto dto) =>
            await _context.RunAsync(
                    @"UPDATE Days SET
                    Monday = @Monday,
                    Tuesday = @Tuesday,
                    Wednesday = @Wednesday,
                    Thursday = @Thursday,
                    Friday = @Friday,
                    Saturday = @Saturday,
                    Sunday = @Sunday
                    WHERE ProfileId = @ProfileId",
                    new DynamicParameters().Fill(
                       ("Monday", dto.Monday, DbType.Boolean),
                       ("Tuesday", dto.Tuesday, DbType.Boolean),
                       ("Wednesday", dto.Wednesday, DbType.Boolean),
                       ("Thursday", dto.Thursday, DbType.Boolean),
                       ("Friday", dto.Friday, DbType.Boolean),
                       ("Saturday", dto.Saturday, DbType.Boolean),
                       ("Sunday", dto.Sunday, DbType.Boolean),
                       ("ProfileId", id, DbType.String)));

        public async Task DeleteProfile(params DynamicParameters[] obj)
        {
            await _context.RunAsync(
                async (c, t) => await c.ExecuteAsync(
                    @"DELETE FROM Profile WHERE Id = @ProfileId", obj[0], t),
                async (c, t) => await c.ExecuteAsync(
                    @"DELETE FROM Days WHERE ProfileId = @ProfileId", obj[1], t));

            _logger.Information($"User Profile Deleted at {DateTime.Now} with id {obj}");
        }

        public async Task InitialiseProfile(params DynamicParameters[] obj)
        {
            await _context.RunAsync(
                async (c, t) => await c.ExecuteAsync(
                    @"INSERT INTO Profile (Id, Role, Username)
            VALUES (@ProfileId, @Role, @Username)", obj[0], t),
                async (c, t) => await c.ExecuteAsync(
                    @"INSERT INTO Days (ProfileId)
            VALUES (@ProfileId)", obj[1], t),
                async (c, t) => await c.ExecuteAsync(
                    @"INSERT INTO Location (ProfileId)
            VALUES (@ProfileId)", obj[2], t));

            _logger.Information($"User Profile Created at {DateTime.Now} with id {obj}");
        }
    }
}