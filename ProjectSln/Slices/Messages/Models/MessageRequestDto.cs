namespace Main.Slices.Messages.Models
{
    public record MessageRequestDto : MessageDto
    {
        public MessageRequestDto(DateTime Date, string SenderId, string Body, string ActorName, string TargetName) : base(Date, SenderId, Body)
        {
            this.Date = Date;
            this.SenderId = SenderId;
            this.Body = Body;
            this.ActorName = ActorName;
            this.TargetName = TargetName;
        }


        public DateTime Date { get; set; }
        public string SenderId { get; set; }
        public string Body { get; set; }
        public string ActorName { get; set; }
        public string TargetName { get; set; }

    }
}
