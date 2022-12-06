using Homestead.Slices.Rota.Services.Rota;
using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Db
{
    public record ShiftModel
    {
        public ShiftModel(string ClubName, string ClubId, DateTime Start, DateTime End, EventStatus Status, IEnumerable<Security> Personel){

            this.ClubName = ClubName;

            this.ClubId = ClubId;
            
            this.Start = Start;

            this.End = End;

            this.Status = Status;

            this.Personel = Personel;


        }


        public string ClubName { get; set; }

        public string ClubId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public EventStatus Status { get; set; }

        public IEnumerable<Security> Personel { get; set; }

    }

    public record class Security
    {
        public Security(string SecurityName, string SecurityId, Attendance Attendance)
        {
            this.SecurityName = SecurityName;
            this.SecurityId = SecurityId;
            this.Attendance = Attendance;
        }

        public string SecurityName { get; init; }
        public string SecurityId { get; init; }
        public Attendance Attendance { get; set; }

    }
}
