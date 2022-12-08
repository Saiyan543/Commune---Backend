using Main.Global.Library.ApiController.Responses;

namespace Main.Slices.Accounts.Models.Responses
{
    public sealed class AccountNotFoundResponse : NotFoundResponse
    {
        public AccountNotFoundResponse(string id)
            : base($"Account with Id: {id} Not found. Time: {DateTime.UtcNow}")
        { }
    }
}