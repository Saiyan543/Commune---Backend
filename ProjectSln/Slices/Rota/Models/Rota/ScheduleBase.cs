namespace Main.Slices.Rota.Models.Rota
{
    public abstract record class ScheduleBase
    {
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }

        public ScheduleBase( DateTime? Start, DateTime? End)
        {
            this.Start = Start;
            this.End = End;
     
        }
    }
}