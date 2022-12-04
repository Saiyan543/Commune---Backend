using Homestead.Slices.Rota.Services.Rota;
using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Db
{
    public sealed class ClubShiftModel
    {
        public List<Security>? Personel { get; set; }
    }

    public record Security(string Name, string Id, Attendance Status);
}
