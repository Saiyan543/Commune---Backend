using Main.Slices.Rota.Models.Rota.enums;

namespace Main.Slices.Rota.Models.Rota
{
    public record class UpdateAttendanceDto
    {
        public UpdateAttendanceDto(string clubId, DateTime date, int AttendanceId)
        {
            this.ClubId = clubId;
            this.Date = date;
            this.AttendanceId = AttendanceId;
        }

        public DateTime Date { get; init; }
        public int AttendanceId { get; set; }
        public string ClubId { get; set; }

        public Attendance Attendance => AttendanceId switch
        {
            1 => Attendance.Unconfirmed,
            2 => Attendance.Confirmed,
            3 => Attendance.Declined,
            _ => throw new InvalidOperationException()
        };
    }
}