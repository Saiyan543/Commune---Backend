using FluentMigrator.Runner;
using Main.DataAccessConfig.DapperOrm.Context;

namespace Main.DataAccessConfig.DapperOrm
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app, Serilog.ILogger logger)
        {
            using (var scope = app.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    databaseService.CreateDatabase("Profile");

                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                }
                catch (Exception ex)
                {
                    logger.Error($"Exception occured during the database creation: {ex}");
                    throw;
                }
            }
            return app;
        }
    }
}