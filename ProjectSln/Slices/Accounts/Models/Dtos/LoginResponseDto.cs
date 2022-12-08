namespace Main.Slices.Accounts.Models.Dtos
{
    public record LoginResponseDto(TokenDto token, string userId);
}