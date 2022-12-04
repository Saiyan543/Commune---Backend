namespace Main.Slices.Accounts.Dependencies.Jwt.Configuration.Models.Dtos
{
    public record TokenDto(string AccessToken, string RefreshToken);
}