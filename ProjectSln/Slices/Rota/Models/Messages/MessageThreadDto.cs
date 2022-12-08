namespace Main.Slices.Rota.Models.Messages
{
    public record MessageThreadDto(IEnumerable<MessageDto> MessagesThreads, string name, string otherPartyId);
}