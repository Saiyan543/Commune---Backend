using Main.Slices.Rota.Models.Dtos;
using Main.Slices.Rota.Models.Dtos.In;

namespace Main.Slices.Rota.Services.Message
{
    public interface IMessageService
    {
        Task AppendMessageThread(string actorId, string targetId, MessageDto message);

        Task DeleteThread(string actorId, string targetId);

        Task RespondToMessageRequest(string actorId, string targetId, MessageRequestResponse response);

        Task<IEnumerable<MessageDto>?> RetrieveMessageRequests(string id);
        Task<IEnumerable<MessageThreadDto>> RetrieveThreads(string id);
        Task SendMessageRequest(string actorId, string targetId, string message);
    }
}