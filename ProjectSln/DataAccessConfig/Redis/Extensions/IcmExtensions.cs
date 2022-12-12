using Newtonsoft.Json;
using StackExchange.Redis;

namespace Main.DataAccessConfig.Redis.Extensions
{
    public static class ICMExtensions
    {
        public static async Task Write(this IDatabase _redis, params Func<ITransaction, Task>[] executions)
        {
            var trans = _redis.CreateTransaction();
            foreach (var exc in executions)
            {
                exc(trans);
            }
            await trans.ExecuteAsync();
        }

        public static T Deserialize<T>(this RedisValue value)
        {
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static IEnumerable<T> Deserialize<T>(this RedisValue[] values)
        {
            foreach (var val in values)
            {
                yield return JsonConvert.DeserializeObject<T>(val);
            }
        }
    }
}