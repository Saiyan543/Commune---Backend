namespace Main.Slices.Accounts.EntityFramework_Jwt
{
    public sealed class JwtOptions
    {
        public string Section { get; set; } = "JwtOptions";
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Expires { get; set; }
    }
}