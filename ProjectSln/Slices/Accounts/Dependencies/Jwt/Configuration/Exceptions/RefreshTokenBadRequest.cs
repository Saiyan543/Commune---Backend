using Main.Global.Library.GlobalExceptionHandling.Exceptions;

namespace Main.Slices.Accounts.Dependencies.Jwt.Configuration.Exceptions
{
    public sealed class RefreshTokenBadRequest : BadRequestException
    {
        public RefreshTokenBadRequest()
            : base("Invalid client request. The tokenDto has some invalid values.")
        { }
    }
}