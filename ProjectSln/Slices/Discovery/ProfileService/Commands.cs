using AutoMapper;
using Dapper;
using Main.Global.Helpers.Location.Coordinates;
using Main.Slices.Discovery.DapperOrm.Context;
using Main.Slices.Discovery.DapperOrm.Extensions;
using Main.Slices.Discovery.Models.Dtos;
using Main.Slices.Discovery.ProfileService;
using System.Data;

namespace Homestead.Slices.Discovery.ProfileService
{
    public partial class ProfileService : IProfileService
    {
        private readonly IMapper _mapper;
        private readonly DapperContext _context;
        private readonly Serilog.ILogger _logger;

        public ProfileService(Serilog.ILogger logger, IMapper mapper, DapperContext context)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
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

        public async Task DeleteProfile(params DynamicParameters[] obj) =>
            await _context.RunAsync(
                async (c, t) => await c.ExecuteAsync(
                    @"DELETE FROM Profile WHERE Id = @ProfileId", obj[0], t),
                async (c, t) => await c.ExecuteAsync(
                    @"DELETE FROM Days WHERE ProfileId = @ProfileId", obj[1], t));

        public async Task InitialiseProfile(params DynamicParameters[] obj) =>
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
    }
}