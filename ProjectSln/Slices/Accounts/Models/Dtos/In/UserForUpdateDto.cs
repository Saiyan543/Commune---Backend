using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos.In
{
    public record UserForUpdateDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "Email name is required")]
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
    }
}