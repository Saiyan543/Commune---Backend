using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Main.Slices.Rota.Dependencies.Redis.Extensions
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

        public static T RedisDeserialize<T>(this RedisValue value)
        {
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static IEnumerable<T> RedisDeserialize<T>(this RedisValue[] values)
        {
            List<T> ss = new();
            foreach (var val in values)
            {
                ss.Append(JsonConvert.DeserializeObject<T>(val));
            }
            return ss;
        }


        public static async Task<IEnumerable<T>> GetRecordsAsync<T>(this IDatabase redis, string key)
        {
            var result = await redis.StringGetAsync(key);
            if (result == RedisValue.Null)
                return Enumerable.Empty<T>();

            return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDatabase redis, string key)
        {
            var result = await redis.StringGetAsync(key);
            if (result == default)
                return default;

            return JsonConvert.DeserializeObject<T>(result);
        }


        public static async Task SetRecordAsync<T>(this IDatabase redis, string key, T value, TimeSpan? expiry)
        {
            string store = JsonConvert.SerializeObject(value);
            await redis.StringSetAsync(key, store, expiry);
        }



    }
}