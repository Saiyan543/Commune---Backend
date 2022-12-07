using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Dtos.In
{
    public record class UpdatePersonelAttendanceDto
    {
        public UpdatePersonelAttendanceDto(string RotaId, int AttendanceId)
        {
            this.RotaId = RotaId;
            this.AttendanceId = AttendanceId;
        }

        public string RotaId { get; init; }
        public int AttendanceId { get; set; }

        public Attendance AttendanceOut() => AttendanceId switch
        {
            1 => Attendance.Unconfirmed,
            2 => Attendance.Confirmed,
            _ => throw new InvalidOperationException()
        };

    }
}
