namespace Main.Global.Library.RabbitMQ.Options
{
    public sealed record RabbitMqQueueOptions(string queueName, string exchangeName, string routingKey);
}