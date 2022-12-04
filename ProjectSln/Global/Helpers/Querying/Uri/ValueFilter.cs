namespace Main.Global.Helpers.Querying.Uri
{
    public record ValueFilter
    {
        public string? Field { get; set; }

        public string? Value { get; set; }

        public string? Operator { get; set; }
    }
}