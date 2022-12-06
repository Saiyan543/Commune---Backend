namespace Main.Slices.Rota.Models.Base
{
    public abstract class UpcomingRota
    {
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }
        public DateTime Date { get; init; }
        public UpcomingRota(DateTime? Start, DateTime? End, DateTime date)
        {
            this.Start = Start;
            this.End = End;
            this.Date = date;
        }



    }
}
