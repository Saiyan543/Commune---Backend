using Newtonsoft.Json;
using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;

namespace Main.Global.Library.RabbitMQ.Messages
{
    public abstract class BaseMessage
    {
        [JsonConstructor]
        public BaseMessage(string ExchangeName, string RoutingKey)
        {
            this.ExchangeName = ExchangeName;
            this.RoutingKey = RoutingKey;
        }

        [JsonProperty]
        public string ExchangeName;

        [JsonProperty]
        public string RoutingKey;
    }
}