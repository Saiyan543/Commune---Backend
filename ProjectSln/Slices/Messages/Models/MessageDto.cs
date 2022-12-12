namespace Main.Slices.Messages.Models
{
    public record MessageDto(DateTime Date, string SenderId, string Body);
}