using Homestead.Slices.Rota.Services.Rota;
using Main.Slices.Rota.Models.Base;

namespace Main.Slices.Rota.Models.Db
{
    public class SecurityUpcomingRota : UpcomingRota
    {
        public SecurityUpcomingRota(DateTime? Start, DateTime? End, DateTime date) : base(Start, End, date)
        { }
    }
}

