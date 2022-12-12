namespace Main.Slices.Profile.Models.Dtos
{
    public record ProfileDto
    {
        public bool ShowInSearch { get; init; }
        public bool ActivelyLooking { get; init; }
        public string Bio { get; init; } = string.Empty;
    }
}