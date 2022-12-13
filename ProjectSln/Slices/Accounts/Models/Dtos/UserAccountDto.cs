namespace Main.Slices.Accounts.Models.Dtos
{
    public record UserAccountDto
    {
        public string Id { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public int PhoneNumber { get; init; }
    }
}