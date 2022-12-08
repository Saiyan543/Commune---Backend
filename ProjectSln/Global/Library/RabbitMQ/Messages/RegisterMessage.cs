using Newtonsoft.Json;

namespace Main.Global.Library.RabbitMQ.Messages
{
    public sealed class RegisterMessage : BaseMessage
    {
        public RegisterMessage(string id, string username, string type)
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