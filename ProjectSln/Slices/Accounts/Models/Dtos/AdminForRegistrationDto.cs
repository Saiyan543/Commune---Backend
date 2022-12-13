using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record AdminForRegistrationDto : UserForRegistrationDto
    {
        [Required(ErrorMessage = "Admin password required")]
        public string AdminPassword { get; set; } = string.Empty;

        [Range(3,3, ErrorMessage = "Only Admin Role valid at this endpoint")]
        public new int RoleId { get; init; } = 3;
    }
}