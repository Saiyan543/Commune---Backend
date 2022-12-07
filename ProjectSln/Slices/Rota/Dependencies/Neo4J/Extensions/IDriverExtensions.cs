using Neo4j.Driver;

namespace Main.Slices.Rota.Dependencies.Neo4J.Extensions
{
    public static class IDriverExtensions
    {
        private const string db = "neo4j";

        public static async Task<TResult> ReadSingleAsync<TResult>(this IDriver driver, string query, object parameters, Func<IRecord, TResult> operation)
        {
            using (var session = driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase(db)))
            {
                var record = await session.RunAsync(query, parameters).ContinueWith(async x => await x.Result.SingleAsync());
                return operation(await record);
            }
        }

        public static async Task<IEnumerable<TResult>> ReadAsync<TResult>(this IDriver driver, string query, object? parameters, Func<IRecord, TResult> operation)
        {
            using (var session = driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase(db)))
            {
                var result = await session.RunAsync(query, parameters);
                var records = await result.ToListAsync();

                return records.Select(x => operation(x));
            }
        }

        public static async Task RunAsync(this IDriver driver, string command, object parameters)
        {
            using (var session = driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase(db)))
            {
                await session.RunAsync(command, parameters);
            }
        }

        public static async Task<TResult> RunAsync<TResult>(this IDriver driver, Func<IAsyncSession, Task<TResult>> cmd)
        {
            using (var session = driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase(db)))
            {
                return await cmd(session);
            }
        }

        public static async Task Transaction(this IDriver driver, params Func<IAsyncQueryRunner, Task>[] command)
        {
            using (var session = driver.AsyncSession(ConfigBuilder => ConfigBuilder.WithDatabase(db)))
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    foreach (var cmd in command)
                    {
                        await cmd(tx);
                    }
                });
            }
        }

    }
}