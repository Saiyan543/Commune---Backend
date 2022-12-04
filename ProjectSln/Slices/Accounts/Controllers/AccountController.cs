using Main.Global;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Slices.Accounts.Models.Dtos.In;
using Main.Slices.Accounts.Models.Dtos.Out;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public AccountController(IServiceManager services) => _services = services;

        [HttpGet]
        //   [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAccount([FromQuery] string id)
        {
            var result = await _services.Account.GetUserAccountById(id);
            if (!result.Success)
                return ProcessError(result);
            return Ok(result.GetResult<UserAccountDto>());
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> UpdateUserAccount(string id, [FromBody] UserForUpdateDto inDto)
        {
            var result = await _services.Account.UpdateUser(id, inDto);
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
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] UpdatePasswordDto inDto)
        {
            var result = await _services.Account.UpdatePassword(id, inDto);
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

        [HttpPost]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> RegisterUserAccount([FromBody] UserForRegistrationDto inDto)
        {
            var result = await _services.Account.RegisterUser(inDto);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
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