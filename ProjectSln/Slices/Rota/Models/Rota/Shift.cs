using Main.Slices.Rota.Models.Rota.enums;

namespace Main.Slices.Rota.Models.Rota
{
    public class Shift
    {
        public Shift(string? ClubId, string? ClubName, DateTime? Start, DateTime? End, EventStatus Status, List<Personel> Personel)
        {
            this.ClubName = ClubName == null ? string.Empty : ClubName;

            this.ClubId = ClubId == null ? string.Empty : ClubId;

            this.Start = Start;

            this.End = End;

            this.Status = Status;

            this.Personel = Personel;
        }

        public string ClubName { get; set; }

        public string ClubId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public EventStatus Status { get; set; }

        public List<Personel> Personel { get; set; }
    }
}