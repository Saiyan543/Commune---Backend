using Homestead.Slices.Accounts.Models.Dtos.Out;
using Main.Global;
using Main.Global.Library.ApiController;
using Main.Slices.Accounts.Dependencies.Jwt.Configuration.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public AuthenticationController(IServiceManager services) => _services = services;

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticationDto user)
        {
            var userId = await _services.Authentication.ValidateUser(user);

            if (userId is null)
                return Unauthorized();

            var tokenDto = await _services.Authentication
                .CreateToken(populateExp: true);

            return StatusCode(201, new LoginResponseDto(tokenDto, userId));
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var baseResponse = await _services.Authentication.RefreshToken(tokenDto);
            if (!baseResponse.Success)
                return ProcessError(baseResponse);
            return StatusCode(201, baseResponse.GetResult<TokenDto>());
        }
    }
}