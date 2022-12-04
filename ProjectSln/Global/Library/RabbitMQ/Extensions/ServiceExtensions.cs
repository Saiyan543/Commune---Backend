using Main.Global.Library.RabbitMQ;
using Main.Global.Library.RabbitMQ.Subscribers;

namespace Main.Global.Library.RabbitMQ.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRabbitMqPublisher(this IServiceCollection services) =>
            services.AddSingleton<IPublisher, Publisher>();

        public static void ConfigureRabbitMqSubscribers(this IServiceCollection services)
        {
            services.AddHostedService<RegisterSubscriber>();
            services.AddHostedService<DeleteSubscriber>();
        }
    }
}