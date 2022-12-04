using Main.Slices.Rota.Dependencies.Neo4J.Extensions;
using Main.Slices.Rota.Services.Contract;
using Neo4j.Driver;

namespace Homestead.Slices.Rota.Services
{
    public partial class ContractService : IContractService
    {
        private readonly IDriver _driver;

        public ContractService(IDriver driver) => _driver = driver;

        public async Task SendContractRequest(string actorId, string targetId) =>
                await _driver.RunAsync("MATCH (a:User{id:$actorId}) MATCH (b:User{id:$targetId}) MERGE (a)-[:Contract{status:'pending'}]-(b)", new { actorId = actorId, targetId = targetId });

        public async Task RespondToContractRequest(string actorId, string targetId, string response) =>
            await _driver.RunAsync(@"MATCH (a:User{id:$actorId})-[r:Contract{status:'pending'}]-(b:User{id:$targetId}) SET r.status = $response", new { actorId = actorId, targetId = targetId, response = response });

        public async Task DeleteContract(string actorId, string targetId) =>
         await _driver.RunAsync(@"MATCH (a:User{id:$actorId})-[r:Contract]-(b:User{id:$targetId}) DETACH DELETE r", new { actorId = actorId, targetId = targetId });

        public async Task InitialiseNode(string id, string name, string role) =>
          await _driver.RunAsync(@"MERGE (:User{id:$id, name:$name, role:$role})", new { id = id, name = name, role = role });

        public async Task DeleteNode(string id) =>
            await _driver.RunAsync("MATCH (a:User{id:$id}) DETACH DELETE a", new { id = id });
    }
}