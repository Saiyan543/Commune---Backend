using Homestead.Slices.Rota.Services.Rota;
using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Db
{
    public record SecurityShiftModel(string ClubName, string ClubId, DateTime Start, DateTime End, Attendance Status);
}
