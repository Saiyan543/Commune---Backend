namespace Main.Slices.Discovery.Models.Dtos
{
    public record struct PostcodeDto
    {
        public string Value { get; init; }

        public PostcodeDto(string PostCode)
        {
            this.Value = PostCode;
        }

        public bool ValidatePostCode() // proxy for real validation
        {
            if (string.IsNullOrEmpty(Value))
                return false;
            if (Value.Length > 8 && Value.Length < 6)
                return false;
            if (!Value.Contains(" "))
                return false;
            return true;
        }
    }
}