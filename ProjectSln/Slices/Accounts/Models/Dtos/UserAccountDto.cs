namespace Main.Slices.Accounts.Models.Dtos
{
    public record UserAccountDto
    {
        public string Id { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }
}