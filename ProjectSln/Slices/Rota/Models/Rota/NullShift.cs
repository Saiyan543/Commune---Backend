using Main.Slices.Rota.Models.Rota.enums;

namespace Main.Slices.Rota.Models.Rota
{
    public class NullShift : Shift
    {
        public NullShift()
            : base(null, null, null, null, EventStatus.Null, Enumerable.Empty<Personel>().ToList())
        { }
    }
}