namespace Main.Slices.Messages.Messages
{
    public record MessageDto
    {
        public DateTime Date { get; init; }
        public string SenderId { get; init; }
        public string Body { get; init; }
    }
}