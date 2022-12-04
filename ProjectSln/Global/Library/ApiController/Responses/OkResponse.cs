namespace Main.Global.Library.ApiController.Responses
{
    public sealed class Response<TResult> : BaseResponse
    {
        public TResult? Result { get; set; }

        public Response(TResult? result)
            : base(true)
        {
            Result = result;
        }
    }
}