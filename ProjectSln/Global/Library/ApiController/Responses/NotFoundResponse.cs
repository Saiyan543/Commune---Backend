namespace Main.Global.Library.ApiController.Responses
{
    public abstract class NotFoundResponse : BaseResponse
    {
        public string Message { get; set; }

        public NotFoundResponse(string message)
            : base(false)
        {
            Message = message;
        }
    }
}