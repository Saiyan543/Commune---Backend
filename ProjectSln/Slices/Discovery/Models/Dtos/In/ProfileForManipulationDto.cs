using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Discovery.Models.Dtos.In
{
    public record ProfileForManipulationDto
    {
        public bool ShowInSearch { get; init; }
        public bool ActivelyLooking { get; init; }
        [Required(ErrorMessage = "Bio required")]
        public string Bio { get; init; } = string.Empty;
    }
}