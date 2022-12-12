namespace Main.Slices.Rota.Models.Rota
{
    public record UpdateSchedule_ClubDto : Schedule_Club
    {
        public UpdateSchedule_ClubDto(DateTime? Start, DateTime? End, int Personel, DateTime date) : base(Start, End, Personel)
        {
            Date = date;
        }
        public DateTime Date { get; init; }
    }
}
