using Main.Slices.Accounts.Dependencies.Jwt.Configuration.Models.Dtos;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record LoginResponseDto(TokenDto token, string userId);
}