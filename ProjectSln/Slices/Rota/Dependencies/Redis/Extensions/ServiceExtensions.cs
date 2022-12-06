
using Main.Slices.Rota.Dependencies.Redis.Options;
using StackExchange.Redis;

namespace Main.Slices.Rota.Dependencies.Redis.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIConnectionMultiplexor(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = new RedisOptions();
            configuration.Bind("RedisOptions", redisConfig);

            ConfigurationOptions options = new ConfigurationOptions
            {
                // ChannelPrefix = redisConfig.ChannelPrefix,
                // ClientName = redisConfig.ClientName,
                // Password = redisConfig.Password,
                // User = redisConfig.User,
                EndPoints = new EndPointCollection()
                {
                    redisConfig.Hosts.ElementAt(0)+":"+redisConfig.Ports.ElementAt(0)
                }
            };

            services.AddSingleton(sp =>
                ConnectionMultiplexer.Connect(options).GetDatabase());
        }
    }
}