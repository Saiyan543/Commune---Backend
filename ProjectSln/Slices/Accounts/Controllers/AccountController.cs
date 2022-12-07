using Main.Global;
using Main.Global.Helpers;
using Main.Global.Helpers.Querying.Paging;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Slices.Accounts.Dependencies.IdentityCore.Models;
using Main.Slices.Accounts.Models.Dtos;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
   // [Authorize(AuthenticationSchemes = "Bearer")]
    public sealed class AccountController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public AccountController(IServiceManager services) => _services = services;
        
        [HttpGet]
        [Route("Search")]
  //      [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [ServiceFilter(typeof(ValidateMediaTypeFilter))]
        public async Task<IActionResult> SearchUsers([FromQuery] EFCoreQueryDto dto)
        {
            var results = await _services.Account.QueryAccounts(dto, false);

            var paged = PagedResults<UserAccountDto>.Page(results, dto.pageNumber, dto.pageSize);
            Response.Headers.Add("X-Page", paged.MetaData.Serialize());

            return Ok(paged);
        }
        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetUserAccount([FromQuery] string id)
        {
            var result = await _services.Account.GetUserAccountById(id);
            if (!result.Success)
                return ProcessError(result);
            return Ok(result.GetResult<UserAccountDto>());
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateUserAccount(string id, [FromBody] UserForUpdateDto dto)
        {
            var result = await _services.Account.UpdateUser(id, dto);
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

        [HttpPut("password/{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] UpdatePasswordDto dto)
        {
            var result = await _services.Account.UpdatePassword(id, dto);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
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