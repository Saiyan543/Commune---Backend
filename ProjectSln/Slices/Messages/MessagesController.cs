using Main.Global;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.ApiController;
using Main.Global.Library.ApiController.Responses;
using Main.Slices.Messages.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Main.Slices.Messages
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    public sealed class MessagesController : ApiControllerBase
    {
        private readonly IServiceManager _services;
        private readonly IMongoDatabase _db;
        //public MessagesController(IServiceManager services) => _services = services;
        public MessagesController(IServiceManager services)
        {
            _services = services;
          //  _db = MongoDbDriver._db;
        }
        [HttpGet]
     //   [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
       // [HttpCacheValidation(MustRevalidate = true)]
        public async Task<IActionResult> GetMessageThreads([FromQuery] string id)
        {
            var result = await _services.Message.RetrieveMessageThreads(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("request")]
     //   [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        //[HttpCacheValidation(MustRevalidate = true)]
        public async Task<IActionResult> GetMessageRequests([FromQuery] string id)
        {
            var result = await _services.Message.RetrieveMessageRequests(id);
            return Ok(result);

        }

        [HttpPut("request/respond/{actorId}/{targetId}/{accept}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> RespondToRequest(string actorId, string targetId, bool accept)
        {

            await _services.Message.AcceptMessageRequest(actorId,  targetId, accept);
            return NoContent();
        }

        [HttpPut("request/send/{actorId}/{targetId}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> SendRequestMessage(string actorId, string targetId, [FromBody] SubmitMessageDto dto)
        {
  
            await _services.Message.SendMessageRequest(actorId, targetId, new MessageDto(dto));
            return NoContent();
        }

        [HttpPut("{actorId}/{targetId}")]
        [ServiceFilter(typeof(ValidateModelStateFilter))]
        public async Task<IActionResult> AddMessageToThread(string actorId, string targetId, [FromBody] SubmitMessageDto dto)
        {
            await _services.Message.SendMessage(actorId, targetId, new MessageDto(dto));
            return NoContent();
        }

        [HttpDelete("{actorId}/{targetId}")]
        public async Task<IActionResult> DeleteMessageThread(string actorId, string targetId)
        {
            await _services.Message.DeleteKey(actorId, targetId);
            return NoContent();
        }
    }
}