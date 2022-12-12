namespace Main.Slices.Messages.Models
{
    public record MessageThreadDto(string name, IEnumerable<MessageDto> Messages);

}
