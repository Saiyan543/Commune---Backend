namespace Main.Global.Library.RabbitMQ
{
    public static class Constants
    {
        // Exchanges
        public const string Homestead = "BelleThorn";

        // Routing Keys
        public const string Register = "Register";

        public const string Delete = "Delete";

        // Queues
        public const string RegisterQueue = "RegisterQueue";

        public const string DeleteQueue = "Delete";
    }
}