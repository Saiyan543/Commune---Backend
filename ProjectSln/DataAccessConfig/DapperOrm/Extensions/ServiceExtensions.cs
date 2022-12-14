using FluentMigrator.Runner;
using Main.DataAccessConfig.DapperOrm.Context;
using System.Reflection;

namespace Main.DataAccessConfig.DapperOrm.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Database>();
            services.AddSingleton<DapperContext>();

            services.AddLogging(c =>
                    c.AddFluentMigratorConsole())
                .AddFluentMigratorCore().ConfigureRunner(c =>
                    c.AddSqlServer2016().WithGlobalConnectionString(configuration
                            .GetConnectionString("sqlConnection"))
                        .ScanIn(Assembly.GetExecutingAssembly())
                        .For.Migrations());
        }
    }
}