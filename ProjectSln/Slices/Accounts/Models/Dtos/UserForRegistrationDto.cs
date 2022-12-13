using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record UserForRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; init; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; init; }
        [Required(ErrorMessage = "Username is required")]
        [MinLength(8, ErrorMessage = "Username must be at least 8 characters")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; init; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string RepeatPassword { get; init; } 
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; }
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; init; } = 0000000000;

        [Range(1, 4, ErrorMessage = "Valid Role Id between 1,3: 1 = Security, 2 = Establishment, 3 = Admin") /* 1 = Security, 2 = Establishment, 3 = Admin */]
        public int RoleId { get; init; }
    }
}