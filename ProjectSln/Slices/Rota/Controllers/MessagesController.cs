using Main.Global;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Slices.Rota.Models.Messages;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Rota.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    public sealed class MessagesController : ApiControllerBase
    {
        private readonly IServiceManager _services;

        public MessagesController(IServiceManager services) => _services = services;

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = true)]
        public async Task<IActionResult> GetMessageThreads([FromQuery] string id)
        {
            var result = await _services.Message.RetrieveThreads(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("request")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = true)]
        public async Task<IActionResult> GetMessageRequests([FromQuery] string id)
        {
            var result = await _services.Message.RetrieveMessageRequests(id);
            return Ok(result);
        }

        [HttpPut("request/respond/{actorId}/{targetId}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> RespondToRequest(string actorId, string targetId, [FromBody] MessageRequestResponseDto response)
        {
            if (response.Response == string.Empty)
                return BadRequest("Invalid Response");
            await _services.Message.RespondToMessageRequest(actorId, targetId, response);
            return NoContent();
        }

        [HttpPut("request/send/{actorId}/{targetId}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> SendRequestMessage(string actorId, string targetId, [FromBody] MessageDto dto)
        {
            await _services.Message.SendMessageRequest(actorId, targetId, dto.Body);
            return NoContent();
        }

        [HttpPut("{targetId}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
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