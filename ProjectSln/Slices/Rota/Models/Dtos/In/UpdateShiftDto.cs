using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Dtos.In
{
    public record UpdateShiftDto(string ClubId, DateTime Start, DateTime End, EventStatus status);
}
