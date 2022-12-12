using Dapper;
using Main.DataAccessConfig.DapperOrm.Context;
using System.Data;
using static Dapper.SqlMapper;

namespace Main.DataAccessConfig.DapperOrm.Extensions
{
    public static class DapperContextExtensions
    {
        public static async Task<TResult> ReadSingleAsync<TResult>(this DapperContext conn, Func<IDbConnection, Task<TResult>> cmd)
        {
            using (var connection = conn.CreateConnection())
            {
                return await cmd(connection);
            }
        }

        public static async Task<IEnumerable<TResult>> ReadAsync<TResult>(this DapperContext conn, Func<IDbConnection, Task<IEnumerable<TResult>>> cmd)
        {
            using (var connection = conn.CreateConnection())
            {
                var result = await cmd(connection);
                return result;
            }
        }

        public static async Task<TResult> RunAsync<TResult>(this DapperContext conn, Func<IDbConnection, Task<TResult>> cmd)
        {
            using (var connection = conn.CreateConnection())
            {
                return await cmd(connection);
            }
        }

        public static async Task RunAsync(this DapperContext conn, string command, DynamicParameters parameters)
        {
            using (var connection = conn.CreateConnection())
            {
                await connection.ExecuteAsync(command, parameters);
            }
        }

        public static async Task RunAsync(this DapperContext conn, params Func<IDbConnection, IDbTransaction, Task>[] commands)
        {
            using (var connection = conn.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var cmd in commands)
                    {
                        await cmd(connection, transaction);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}