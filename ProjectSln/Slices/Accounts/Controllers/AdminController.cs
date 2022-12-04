using Main.Global;
using Main.Global.Helpers.Querying.Paging;
using Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.Dtos;
using Main.Slices.Accounts.Models.Dtos.Out;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Main.Slices.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IServiceManager _services;

        public AdminController(IServiceManager services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> SearchUsers([FromQuery] EFCoreQueryDto dto)
        {
            var results = await _services.Account.QueryAccounts(dto, false);

            var paged = PagedResults<UserAccountDto>.Page(results, dto.pageNumber, dto.pageSize);
            Response.Headers.Add("X-Page", JsonConvert.SerializeObject(paged.MetaData));

            return Ok(paged);
        }

        [HttpDelete]
        public async Task<IActionResult> AdminDeleteUser([FromQuery] string id)
        {
            var result = await _services.Account.DeleteUser(id);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}