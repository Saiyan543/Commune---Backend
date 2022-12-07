using Main.Global.Library.ApiController.Responses;

namespace Main.Slices.Accounts.Models.Responses
{
    public class RefreshTokenExpiredResponse : BadRequestResponse
    {
        public RefreshTokenExpiredResponse(DateTime? exp) : base($"Refresh token expired at {exp}, re-login to get a new one.")
        { }
    }
}