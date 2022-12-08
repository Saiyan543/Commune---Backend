using Main.Global;
using Main.Global.Helpers;
using Main.Global.Helpers.Location;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Helpers.Querying.Paging;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Slices.Discovery.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Discovery
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    public sealed class ProfileController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public ProfileController(IServiceManager services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> GetProfile([FromQuery] string id)
        {
            var result = await _services.Profile.GetProfile(id);
            if (!result.Success)
                return ProcessError(result);

            return Ok(result.GetResult<(ProfileView, PostcodeDto)>().ToTuple());
        }

        [HttpGet]
        [Route("search")]
        //      [ServiceFilter(typeof(RedisCacheFilter))]
        [ServiceFilter(typeof(ValidateMediaTypeFilter))]
        public async Task<IActionResult> SearchProfiles([FromQuery] string id, [FromQuery] ProfileSearchDto dto)
        {
            var result = await _services.Profile.GetCoordinates(id);
            if (!result.Success)
                return ProcessError(result);

            string sql = DynamicQueryBuilder.Build(dto.Comparisons.ToArray(), dto.SplitBools());
            var profiles = await _services.Profile.Search(dto.Range != default ? dto.Range : 5, sql, result.GetResult<Coordinate>());

            var paged = PagedResults<ProfileView>.Page(profiles, dto.PageNumber, dto.PageSize);
            Response.Headers.Add("X-Page", paged.MetaData.Serialize());

            return Ok(paged);
        }

        [HttpPut("profile/{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] ProfileDto dto)
        {
            await _services.Profile.UpdateProfile(id, dto);
            return NoContent();
        }

        [HttpPut("days/{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateDays(string id, [FromBody] DaysDto dto)
        {
            await _services.Profile.UpdateDays(id, dto);
            return NoContent();
        }

        [HttpPut("location/{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateLocation(string id, [FromBody] PostcodeDto dto)
        {
            if (!dto.ValidatePostCode())
                return BadRequest("Invalid Postcode");
            await _services.Profile.UpdateLocation(id, GeoLocation.GetCoordinateFromPostCode(dto.Value));
            return NoContent();
        }
    }
}