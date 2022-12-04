using Main.Global.Library.ApiController.Responses;
using Main.Slices.Accounts.Dependencies.Jwt.Configuration.Models.Dtos;

namespace Main.Slices.Accounts.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<TokenDto> CreateToken(bool populateExp);

        Task<BaseResponse> RefreshToken(TokenDto tokenDto);

        Task<string?> ValidateUser(UserForAuthenticationDto userForAuth);
    }
}