using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Dtos.In
{
    public record class UpdatePersonelAttendanceDto
    {
        public UpdatePersonelAttendanceDto(string RotaId, string SecurityId, Attendance Attendance)
        {
            this.RotaId = RotaId;
            this.SecurityId = SecurityId;
            this.Attendance = Attendance;
        }

        public string RotaId { get; init; }
        public string SecurityId { get; init; }
        public Attendance Attendance { get; set; }

    }
}
