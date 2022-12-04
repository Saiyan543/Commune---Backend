namespace Main.Slices.Rota.Models.Dtos.In
{
    public record MessageDto
    {
        public MessageDto(string SenderId, string Body, DateTime Date)
        {
            this.Date = Date;
            this.SenderId = SenderId;
            this.Body = Body;
        }
        public DateTime Date { get; init; }
        public string SenderId { get; init; }
        public string Body { get; init; }
    }
}