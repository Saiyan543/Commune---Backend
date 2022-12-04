namespace Main.Slices.Discovery.Models.Dtos.In
{
    public struct PostCodeDto
    {
        public string PostCode { get; init; }

        public PostCodeDto(string PostCode)
        {
            this.PostCode = PostCode;
        }

        public bool ValidatePostCode()
        {
            if (string.IsNullOrEmpty(PostCode))
                return false;
            if (PostCode.Length > 8 && PostCode.Length < 6)
                return false;
            if (!PostCode.Contains(" "))
                return false;
            return true;
        }
    }
}