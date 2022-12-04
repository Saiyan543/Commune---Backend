using Main.Global;
using Main.Global.Helpers.Location;
using Main.Global.Helpers.Location.Coordinates;
using Main.Global.Helpers.Querying.Paging;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Slices.Discovery.Models.Dtos.In;
using Main.Slices.Discovery.Models.Dtos.Out;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Main.Slices.Discovery
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public ProfileController(IServiceManager services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> GetProfile([FromQuery] string id)
        {
            var baseResult = await _services.Profile.GetProfile(id);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            return Ok(baseResult.GetResult<ProfileDto>());
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchProfiles([FromQuery] string id, [FromQuery] ProfileSearchDto dto)
        {
            var baseResult = await _services.Profile.GetCoordinates(id);
            if (!baseResult.Success)
                return ProcessError(baseResult);
            string sql = DynamicQueryBuilder.Build(dto.Comparisons.ToArray(), dto.SplitBools());
            var profiles = await _services.Profile.Search(dto.range != default ? dto.range : 5, sql, baseResult.GetResult<Coordinate>());

            var paged = PagedResults<SearchProfileDto>.Page(profiles, dto.PageNumber, dto.PageSize);
            Response.Headers.Add("X-Page", JsonConvert.SerializeObject(paged.MetaData));

            return Ok(paged);
        }

        [HttpPut("profile/{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] ProfileForManipulationDto dto)
        {
            await _services.Profile.UpdateProfile(id, dto);
            return NoContent();
        }

        [HttpPut("days/{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateDays(string id, [FromBody] DaysForManipulationDto dto)
        {
            await _services.Profile.UpdateDays(id, dto);
            return NoContent();
        }

        [HttpPut("location/{id}")]
        public async Task<IActionResult> UpdateLocation(string id, [FromBody] PostCodeDto dto)
        {
            if (!dto.ValidatePostCode())
                return BadRequest("Invalid Postcode");
            await _services.Profile.UpdateLocation(id, GeoLocation.GetCoordinateFromPostCode(dto.PostCode));
            return NoContent();
        }
    }
}