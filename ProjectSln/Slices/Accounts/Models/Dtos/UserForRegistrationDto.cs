using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record UserForRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; init; } = string.Empty;
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; init; } = string.Empty;
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; init; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string RepeatPassword { get; init; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; init; }
    }
}