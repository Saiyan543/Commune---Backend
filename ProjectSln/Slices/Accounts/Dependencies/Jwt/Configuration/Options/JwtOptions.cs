namespace Main.Slices.Accounts.Dependencies.Jwt.Configuration.Options
{
    public class JwtOptions
    {
        public string Section { get; set; } = "JwtOptions";
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Expires { get; set; }
    }
}