using Main.DataAccessConfig.Neo4J.Extensions;
using Main.Global.Helpers;
using Main.Slices.Rota.Models.Contracts;
using Neo4j.Driver;

namespace Main.Slices.Rota.Services
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

    public partial class ContractService : IContractService
    {
        private readonly IDriver _driver;
        private readonly Serilog.ILogger _logger;

        public ContractService(Serilog.ILogger logger, IDriver driver)
        { _driver = driver; _logger = logger; }

        public async Task<IEnumerable<ContractDto>> RetrieveContractRequests(string id)
        {
            var requests = await _driver.ReadAsync(@"MATCH (a:User{id:$id})-[r:Contract{status:'Pending'}]-(b:User) RETURN b.id, b.name", new { id },
                (results) => new ContractDto(results["b.id"].As<string>(), results["b.name"].As<string>()));

            return requests.ResultOrEmpty();
        }

        public async Task<IEnumerable<ContractDto>> RetrieveContracts(string id)
        {
            var contracts = await _driver.ReadAsync(@"MATCH (a:User{id:$id})-[r:Contract{status:'Accepted'}]-(b:User) RETURN b.id, b.name", new { id },
                (results) => new ContractDto(results["b.id"].As<string>(), results["b.name"].As<string>()));
            return contracts.ResultOrEmpty();
        }

        public async Task SendContractRequest(string actorId, string targetId) =>
                await _driver.RunAsync("MATCH (a:User{id:$actorId}) MATCH (b:User{id:$targetId}) MERGE (a)-[:Contract{status:'Pending'}]-(b)",
                    new { actorId, targetId });

        public async Task RespondToContractRequest(string actorId, string targetId, string response) =>
            await _driver.RunAsync(@"MATCH (a:User{id:$actorId})-[r:Contract{status:'Pending'}]-(b:User{id:$targetId}) SET r.status = $response",
                new { actorId, targetId, response });

        public async Task DeleteContract(string actorId, string targetId) =>
         await _driver.RunAsync(@"MATCH (a:User{id:$actorId})-[r:Contract]-(b:User{id:$targetId}) DETACH DELETE r",
             new { actorId, targetId });

        public async Task InitialiseNode(string id, string name, string role)
        {
            await _driver.RunAsync(@"MERGE (:User{id:$id, name:$name, role:$role})",
                new { id, name, role });

            _logger.Information($"User Node Created: {id} {name} {role}");
        }

        public async Task DeleteNode(string id)
        {
            await _driver.RunAsync("MATCH (a:User{id:$id}) DETACH DELETE a",
                new { id });

            _logger.Information($"User Node Deleted: {id}");
        }
    }
}