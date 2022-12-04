namespace Main.Slices.Discovery.Models.Dtos.Base
{
    public record class ProfileBase
    {
        public string ProfileId { get; init; }
        public string Username { get; init; }
        public bool ShowInSearch { get; init; }
        public bool ActivelyLooking { get; init; }
        public string Bio { get; init; }
        public bool Monday { get; init; }
        public bool Tuesday { get; init; }
        public bool Wednesday { get; init; }
        public bool Thursday { get; init; }
        public bool Friday { get; init; }
        public bool Saturday { get; init; }
        public bool Sunday { get; init; }
    }
}