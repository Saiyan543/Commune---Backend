using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record UserForUpdateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(8, ErrorMessage = "Username must be at least 8 characters")]
        public string UserName { get; init; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; }
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; init; } = 0000000000;
    }
}
