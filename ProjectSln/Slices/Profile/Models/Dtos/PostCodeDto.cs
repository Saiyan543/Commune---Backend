namespace Main.Slices.Profile.Models.Dtos
{
    public record struct PostcodeDto
    {
        public string Postcode { get; init; }

        public PostcodeDto(string PostCode)
        {
            Postcode = PostCode;
        }

        public bool ValidatePostCode() // proxy for real validation
        {
            if (string.IsNullOrEmpty(Postcode))
                return false;
            if (Postcode.Length > 8 && Postcode.Length < 6)
                return false;
            if (!Postcode.Contains(" "))
                return false;
            return true;
        }
    }
}