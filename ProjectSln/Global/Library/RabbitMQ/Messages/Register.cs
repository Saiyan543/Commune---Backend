using Newtonsoft.Json;

namespace Main.Global.Library.RabbitMQ.Messages
{
    public sealed class Register : BaseMessage
    {
        public Register(string id, string username, string type)
            : base(Constants.Homestead, Constants.Register)
        {
            ProfileId = id;
            UserName = username;
            Role = type;
        }

        [JsonProperty]
        public string ProfileId { get; init; }

        [JsonProperty]
        public string UserName { get; init; }

        [JsonProperty]
        public string Role { get; init; }
    }
}