using Main.Slices.Rota.Models.Dtos.Out;

namespace Main.Slices.Rota.Services.Contract
{
    public interface IContractService
    {
        Task DeleteContract(string actorId, string targetId);

        Task DeleteNode(string id);

        Task InitialiseNode(string id, string name, string role);

        Task RespondToContractRequest(string actorId, string targetId, string response);

        Task<IEnumerable<ContractDto>> RetrieveContractRequests(string id);

        Task<IEnumerable<ContractDto>> RetrieveContracts(string id);

        Task SendContractRequest(string actorId, string targetId);
    }
}