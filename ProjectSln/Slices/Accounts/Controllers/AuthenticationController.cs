﻿
using Main.Global;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Slices.Accounts.Dependencies.Jwt.Configuration.Models.Dtos;
using Main.Slices.Accounts.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IServiceManager _services;
        private readonly IConfiguration _configuration;
        private readonly Serilog.ILogger _logger;

        public AuthenticationController(IServiceManager services, IConfiguration configuration, Serilog.ILogger logger)
        {
            _services = services;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("admin")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> RegisterAdminAccount([FromBody] AdminForRegistrationDto dto)
        {

            if (dto.AdminPassword != _configuration["AdminPassword"])
            {
                _logger.Warning($"Invalid Administrative Register Attempt: {dto.Email}");
                return BadRequest("Invalid password for Administrative registration.");         
            }         
                  
            var result = await _services.Account.RegisterUser(dto, "Admin");
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

        [HttpPost]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> RegisterUserAccount([FromBody] UserForRegistrationDto dto)
        {
            var role = Role.Switch(dto.RoleId);
            if (role.Equals(string.Empty) | role.Equals("Admin"))
                return BadRequest("Invalid Role");

            var result = await _services.Account.RegisterUser(dto, role);
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

        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticationDto dto)
        {
            var userId = await _services.Authentication.ValidateUser(dto);

            if (userId is null)
                return Unauthorized();

            var tokenDto = await _services.Authentication
                .CreateToken(populateExp: true);

            return StatusCode(201, new LoginResponseDto(tokenDto, userId));
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto dto)
        {
            var result = await _services.Authentication.RefreshToken(dto);
            if (!result.Success)
                return ProcessError(result);
            return StatusCode(201, result.GetResult<TokenDto>());
        }
    }
}