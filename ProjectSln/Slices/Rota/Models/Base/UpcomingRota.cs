using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Base
{
    public abstract class UpcomingRota
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public UpcomingRota(DateTime Start, DateTime End)
        {
            this.Start = Start;
            this.End = End;
        }

    }

    public class SecurityUpcomingRota : UpcomingRota
    {
        public SecurityUpcomingRota(DateTime Start, DateTime End) : base(Start, End)
        {
        }
    }

    public class ClubUpcomingRota : UpcomingRota
    {
        public int Personel { get; init; }
        public ClubUpcomingRota(DateTime Start, DateTime End, int Personel) : base(Start, End)
        {
            this.Personel = Personel;
        }

    }
}
