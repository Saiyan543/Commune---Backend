using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos.In
{
    public record UpdatePasswordDto
    {
        [DataType(DataType.Password)]
        [Required]
        public string CurrentPassword { get; init; } = string.Empty;
        [Required]
        public string NewPassword { get; init; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [Required]
        public string RepeatPassword { get; init; } = string.Empty;
    }
}