namespace Main.Slices.Rota.Models.Rota
{
    public record class Schedule_Club : ScheduleBase
    {
        public int Personel { get; init; }
        public Schedule_Club(DateTime? Start, DateTime? End, int Personel) : base(Start, End)
        {
            this.Personel = Personel;
        }
    }
}