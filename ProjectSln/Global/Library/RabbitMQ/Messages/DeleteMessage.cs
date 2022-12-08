using Newtonsoft.Json;

namespace Main.Global.Library.RabbitMQ.Messages
{
    public sealed class DeleteMessage : BaseMessage
    {
        public DeleteMessage(string id)
            : base(Constants.Homestead, Constants.DeleteQueue)
        {
            ProfileId = id;
        }

        [JsonProperty]
        public string ProfileId { get; init; }
    }
}