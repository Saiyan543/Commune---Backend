using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Profile.Models.Dtos
{
    public record ProfileDto
    {
        public bool ShowInSearch { get; init; }
        public bool ActivelyLooking { get; init; }
        [Required(ErrorMessage = "Bio is required")]
        public string Bio { get; init; } = string.Empty;
    }
}