using Main.Global.Library.ApiController.Responses;

namespace Main.Slices.Accounts.Models.Responses
{
    public class RefreshTokenInvalidResponse : BadRequestResponse
    {
        public RefreshTokenInvalidResponse() : base("Invalid client request. The tokenDto has some invalid values.")
        { }
    }
}