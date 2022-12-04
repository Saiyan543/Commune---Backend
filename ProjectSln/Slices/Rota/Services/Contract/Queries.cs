using Main.Global.Helpers;
using Main.Slices.Rota.Dependencies.Neo4J.Extensions;
using Main.Slices.Rota.Models.Dtos.Out;
using Main.Slices.Rota.Services.Contract;
using Neo4j.Driver;

namespace Homestead.Slices.Rota.Services
{
    public partial class ContractService : IContractService
    {
        public async Task<IEnumerable<ContractDto>> RetrieveContractRequests(string id)
        {
            var requests = await _driver.ReadAsync(@"MATCH (a:User{id:$id})-[r:Contract{status:'pending'}]-(b:User) RETURN b.id, b.name", new { id = id },
                (results) => new ContractDto(results["b.id"].As<string>(), results["b.name"].As<string>()));

            return requests.ResultOrEmpty();
        }

        public async Task<IEnumerable<ContractDto>> RetrieveContracts(string id)
        {
            var contracts = await _driver.ReadAsync(@"MATCH (a:User{id:$id})-[r:Contract{status:'Accepted'}]-(b:User) RETURN b.id, b.name", new { id = id },
                (results) => new ContractDto(results["b.id"].As<string>(), results["b.name"].As<string>()));
            return contracts.ResultOrEmpty();
        }
    }
}