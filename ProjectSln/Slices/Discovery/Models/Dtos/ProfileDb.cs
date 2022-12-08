namespace Main.Slices.Discovery.Models.Dtos
{
    public record ProfileDb
    {
        public string UserName { get; init; } = string.Empty;
        public bool ShowInSearch { get; init; }
        public bool ActivelyLooking { get; init; }
        public string Bio { get; init; } = string.Empty;
        public double Latitute { get; init; }
        public double Longitude { get; init; }
        public bool Monday { get; init; }
        public bool Tuesday { get; init; }
        public bool Wednesday { get; init; }
        public bool Thursday { get; init; }
        public bool Friday { get; init; }
        public bool Saturday { get; init; }
        public bool Sunday { get; init; }
    }
}