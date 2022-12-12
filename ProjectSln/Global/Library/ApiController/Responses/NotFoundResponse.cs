namespace Main.Global.Library.ApiController.Responses
{
    public  class NotFoundResponse : BaseResponse
    {
        public string Message { get; set; }

        public NotFoundResponse(string message)
            : base(false)
        {
            Message = message;
        }
    }
}