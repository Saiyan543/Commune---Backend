namespace Main.Global.Library.ApiController.Responses
{
    public abstract class BaseResponse
    {
        public bool Success { get; set; }

        protected BaseResponse(bool success) => Success = success;
    }
}