using Main.Global;
using Main.Global.Library.ApiController;
using Main.Slices.Rota.Models.Dtos.In;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Rota.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public MessagesController(IServiceManager services) => _services = services;

        [HttpGet]
        public async Task<IActionResult> GetMessageThreads([FromQuery] string id)
        {
            var result = await _services.Message.RetrieveThreads(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("request")]
        public async Task<IActionResult> GetMessageRequests([FromQuery] string id)
        {
            var result = await _services.Message.RetrieveMessageRequests(id);
            return Ok(result);
        }

        [HttpPut("request/respond/{actorId}/{targetId}")]
        public async Task<IActionResult> RespondToRequest(string actorId, string targetId, [FromBody] MessageRequestResponse response)
        {
            if (response.Response == string.Empty)
                return BadRequest("Invalid Response");
            await _services.Message.RespondToMessageRequest(actorId, targetId, response);
            return NoContent();
        }

        [HttpPut("request/send/{actorId}/{targetId}")]
        public async Task<IActionResult> SendRequestMessage(string actorId, string targetId, [FromBody] MessageDto dto)
        {
            await _services.Message.SendMessageRequest(actorId, targetId, dto.Body);
            return NoContent();
        }

        [HttpPut("{targetId}")]
        public async Task<IActionResult> AddMessageToThread(string targetId, [FromBody] MessageDto dto)
        {
            await _services.Message.AppendMessageThread(targetId, dto.SenderId, dto);
            return NoContent();
        }

        [HttpDelete("{actorId}/{targetId}")]
        public async Task<IActionResult> DeleteMessageThread(string actorId, string targetId)
        {
            await _services.Message.DeleteThread(actorId, targetId);
            return NoContent();
        }
    }
}