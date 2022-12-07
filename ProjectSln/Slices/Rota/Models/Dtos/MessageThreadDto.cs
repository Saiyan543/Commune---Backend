using Main.Slices.Rota.Models.Dtos.In;

namespace Main.Slices.Rota.Models.Dtos
{
    public record MessageThreadDto(IEnumerable<MessageDto> MessagesThreads, string name, string otherPartyId);
}