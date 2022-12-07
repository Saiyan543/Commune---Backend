using Main.Slices.Rota.Models.enums;

namespace Main.Slices.Rota.Models.Dtos.In
{
    public class UpdateShiftDto
    {
        public UpdateShiftDto(DateTime Start, DateTime End, int EventStatusId)
        {


            this.Start = Start;
            this.End = End;
            this.EventStatusId = EventStatusId;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int EventStatusId { get; set; }

        public EventStatus EventStatus => (EventStatus)EventStatusId;


    }
}
