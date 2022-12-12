using Main.Global;
using Main.Global.Library.ActionFilters;
using Main.Slices.Rota.Models.Contracts;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Rota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    public sealed class ContractController : ControllerBase
    {
        private readonly IServiceManager _services;

        public ContractController(IServiceManager services) => _services = services;

        [HttpGet]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        //[HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> RetrieveContracts([FromQuery] string id)
        {
            var result = await _services.Contract.RetrieveContracts(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("requests")]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        //[HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> RetrieveContractRequests([FromQuery] string id)
        {
            var result = await _services.Contract.RetrieveContractRequests(id);
            return Ok(result);
        }

        [HttpPut("{actorId}/{targetId}/{responseId}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> RespondToContractRequest(string actorId, string targetId, int responseId)
        {
            var response = Enums.ContractResponse(responseId);
            if (response == string.Empty)
                return BadRequest("Invalid Response");
            await _services.Contract.RespondToContractRequest(actorId, targetId, response);
            return NoContent();
        }

        [HttpPost("{actorId}/{targetId}")]
        public async Task<IActionResult> SendContractRequest(string actorId, string targetId)
        {
            await _services.Contract.SendContractRequest(actorId, targetId);
            return NoContent();
        }

        [HttpDelete("{actorId}/{targetId}")]
        public async Task<IActionResult> DeleteContract(string actorId, string targetId)
        {
            await _services.Contract.DeleteContract(actorId, targetId);
            return NoContent();
        }
    }
}