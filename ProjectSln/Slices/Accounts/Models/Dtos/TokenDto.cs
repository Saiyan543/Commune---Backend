using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Accounts.Models.Dtos
{
    public record TokenDto
    {
        public TokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        [MinLength(50, ErrorMessage = "Invalid Token Model")]
        public string AccessToken { get; set; }
        [MinLength(20, ErrorMessage = "Invalid Token Model")]
        public string RefreshToken { get; set; }
     
    }
}