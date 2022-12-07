using Main.Global.Helpers;
using Main.Slices.Discovery.Models.Dtos;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;

namespace Main.Global.Library.ActionFilters
{
    public sealed class RedisCacheFilter : IActionFilter
    {
        public readonly IDatabase _db;
        public string? query { get; set; }
        public RedisCacheFilter(IDatabase db) => this._db = db;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            query = context.ActionArguments["dto"]?.ToString() + context.ActionArguments["id"];
        
            var res = _db.StringGet(query);

            if (!string.IsNullOrEmpty(res))
            {
                context.Result = new OkObjectResult(res.Deserialize<ProfileDto>());
                return;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) => _db.StringSet(query, context.Result.Serialize());
        
    }
}
