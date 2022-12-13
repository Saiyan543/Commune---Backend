using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Main.Slices.Messages.Models
{
    public record MessageDto
    {
        [JsonConstructor]
        public MessageDto(DateTime Date, string SenderId, string Body)
        {

            this.Date = Date;
            this.SenderId = SenderId;
            this.Body = Body;
        }
        public DateTime Date { get; init; }
        public string SenderId { get; init; }
        public string Body { get; init; }
        
        public MessageDto(SubmitMessageDto dto)
        {

            this.Date = dto.Date;
            this.SenderId = dto.SenderId;
            this.Body = dto.Body;
        }

    }
}