namespace Main.Global.Library.RabbitMQ.Options
{
    public sealed record RabbitMqFactoryOptions
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? HostName { get; set; }

        public int Port { get; set; }

        public string? VirtualHost { get; set; }
    }
}