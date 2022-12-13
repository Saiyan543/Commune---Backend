using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record UserForAuthenticationDto
    {
        [MinLength(8, ErrorMessage = "Invalid Username")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "Invalid Password")]
        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}