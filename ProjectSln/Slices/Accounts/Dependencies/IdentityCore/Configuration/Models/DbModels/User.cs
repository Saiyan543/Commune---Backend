using Microsoft.AspNetCore.Identity;

namespace Main.Slices.Accounts.Dependencies.IdentityCore.Configuration.Models.DbModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}