using Main.Slices.Messages.Messages;

namespace Main.Slices.Messages.Models
{
    public record MessageThreadDto(IEnumerable<MessageDto> Messages)

}
