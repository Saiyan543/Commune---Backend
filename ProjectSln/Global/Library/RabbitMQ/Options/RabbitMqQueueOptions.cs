namespace Main.Global.Library.RabbitMQ.Options
{
    public record RabbitMqQueueOptions(string queueName, string exchangeName, string routingKey);
}