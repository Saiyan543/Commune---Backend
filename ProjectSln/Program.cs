using AspNetCoreRateLimit;
using Main.Global;
using Main.Global.Library.ActionFilters;
using Main.Global.Library.GlobalExceptionHandling.Extensions;
using Main.Global.Library.MediaTypes;
using Main.Global.Library.RabbitMQ.Extensions;
using Main.Slices.Accounts.Dependencies.IdentityCore.Extensions;
using Main.Slices.Accounts.Dependencies.Jwt.Extensions;
using Main.Slices.Discovery.DapperOrm;
using Main.Slices.Discovery.DapperOrm.Extensions;
using Main.Slices.Rota.Dependencies.Neo4J.Extensions;
using Main.Slices.Rota.Dependencies.Redis.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IServiceManager, ServiceManager>();

  
        builder.Services.ConfigureCors(builder.Configuration);
        builder.Services.ConfigureSerilogSeq(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.ConfigureIISIntegration();
        
        builder.Services.AddResponseCaching();
        builder.Services.ConfigureHttpCacheHeaders();
        


        builder.Services.ConfigureDapper(builder.Configuration);
       
        builder.Services.ConfigureJwt(builder.Configuration);
        builder.Services.ConfigureEFCoreSqlServer(builder.Configuration);
        builder.Services.ConfigureIdentityStores();
        
        builder.Services.ConfigureRabbitMqPublisher();
        builder.Services.ConfigureRabbitMqSubscribers();


        builder.Services.ConfigureIConnectionMultiplexor(builder.Configuration);

        builder.Services.ConfigureNeo4jDatabase(builder.Configuration);
       builder.Services.ConfigureRateLimitingOptions();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        builder.Services.AddScoped<ValidateModelStateFilter>();
        builder.Services.AddScoped<ValidateMediaTypeFilter>();
        builder.Services.AddScoped<RedisCacheFilter>();
        builder.Services.AddAuthentication();
       
        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
        }).AddXmlDataContractSerializerFormatters()
          .AddCustomCSVFormatters();
        builder.Services.AddCustomMediaTypes();

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<Serilog.ILogger>();
        app.ConfigureGlobalExceptionHandler(logger);
        app.MigrateDatabase(logger);

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseHttpsRedirection();
        app.UseIpRateLimiting();
        app.UseResponseCaching();
        app.UseHttpCacheHeaders();

        app.UseCors("CorsPolicy");
        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}