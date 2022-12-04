using Newtonsoft.Json;

namespace Main.Global.Library.RabbitMQ.Messages
{
    public class Delete : BaseMessage
    {
        public Delete(string id)
            : base(Constants.Homestead, Constants.DeleteQueue)
        {
            ProfileId = id;
        }

        [JsonProperty]
        public string ProfileId { get; init; }
    }
}