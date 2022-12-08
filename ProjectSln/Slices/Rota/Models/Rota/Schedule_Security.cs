namespace Main.Slices.Rota.Models.Rota
{
    public sealed record class Schedule_Security : ScheduleBase
    {
        public Schedule_Security(DateTime? Start, DateTime? End) : base(Start, End)
        {
        }
    }
}