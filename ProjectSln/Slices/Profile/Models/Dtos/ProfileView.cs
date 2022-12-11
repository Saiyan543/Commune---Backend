namespace Main.Slices.Discovery.Models.Dtos
{
    public record ProfileView
    {
        public string Username { get; init; } = string.Empty;
        public bool ActivelyLooking { get; init; }
        public string Bio { get; init; } = string.Empty;
        public bool Monday { get; init; }
        public bool Tuesday { get; init; }
        public bool Wednesday { get; init; }
        public bool Thursday { get; init; }
        public bool Friday { get; init; }
        public bool Saturday { get; init; }
        public bool Sunday { get; init; }
    }
}