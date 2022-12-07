using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record AdminForRegistrationDto : UserForRegistrationDto
    {
        [Required(ErrorMessage = "Admin password required")]
        public string AdminPassword { get; set; } = string.Empty;
    }
}
