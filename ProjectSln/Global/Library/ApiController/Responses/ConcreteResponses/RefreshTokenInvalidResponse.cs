using Main.Global.Library.ApiController.Responses;

namespace Main.Global.Library.ApiController.Responses.ConcreteResponses
{
    public class RefreshTokenInvalidResponse : BadRequestResponse
    {
        public RefreshTokenInvalidResponse() : base("Invalid client request. The tokenDto has some invalid values.")
        { }
    }
}