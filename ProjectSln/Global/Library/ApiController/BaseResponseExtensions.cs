using Main.Global.Library.ApiController.Responses;

namespace Main.Global.Library.ApiController
{
    public static class ApiBaseResponseExtensions
    {
        public static TResultType GetResult<TResultType>(this BaseResponse apiBaseResponse) =>
            ((Response<TResultType>)apiBaseResponse).Result;
    }
}