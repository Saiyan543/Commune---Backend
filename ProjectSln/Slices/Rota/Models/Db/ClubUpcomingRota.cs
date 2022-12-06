using Homestead.Slices.Rota.Services.Rota;
using Main.Slices.Rota.Models.Base;

namespace Main.Slices.Rota.Models.Db
{
    public class ClubUpcomingRota : UpcomingRota
    { 
        public ClubUpcomingRota(DateTime? Start, DateTime? End, DateTime date, int personel) : base(Start, End, date)
        {
            this.Personel = Personel;
        }

        public int Personel { get; set; }
    }
}
