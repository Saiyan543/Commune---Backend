﻿using Main.Global;
using Main.Slices.Rota.Models.Dtos.In;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Rota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IServiceManager _services;

        public ContractController(IServiceManager services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> RetrieveContracts([FromQuery] string id)
        {
            var result = await _services.Contract.RetrieveContracts(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("requests")]
        public async Task<IActionResult> RetrieveContractRequests([FromQuery] string id)
        {
            var result = await _services.Contract.RetrieveContractRequests(id);
            return Ok(result);
        }

        [HttpPut("{actorId}/{targetId}")]
        public async Task<IActionResult> RespondToContractRequest(string actorId, string targetId, [FromBody] ContractRequestResponse dto)
        {
            if (dto.Response == string.Empty)
                return BadRequest("Invalid Response");
            await _services.Contract.RespondToContractRequest(actorId, targetId, dto.Response);
            return NoContent();
        }

        [HttpPost("{actorId}/{targetId}")]
        public async Task<IActionResult> SendContractRequest(string actorId, string targetId)
        {
            await _services.Contract.SendContractRequest(actorId, targetId);
            return StatusCode(201);
        }

        [HttpDelete("{actorId}/{targetId}")]
        public async Task<IActionResult> DeleteContract(string actorId, string targetId)
        {
            await _services.Contract.DeleteContract(actorId, targetId);
            return NoContent();
        }
    }
}