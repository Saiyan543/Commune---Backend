using Main.Slices.Rota.Models.Rota.enums;

namespace Main.Slices.Rota.Models.Rota
{
    public record class Personel
    {
        public Personel(string SecurityId, string SecurityName, Attendance Attendance)
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