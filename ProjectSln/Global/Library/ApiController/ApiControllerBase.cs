using Main.Global.Library.ApiController.Responses;
using Main.Global.Library.GlobalExceptionHandling;
using Microsoft.AspNetCore.Mvc;

namespace Main.Global.Library.ApiController
{
    public class ApiControllerBase : ControllerBase
    {
        public IActionResult ProcessError(BaseResponse baseResponse)
        {
            return baseResponse switch
            {
                NotFoundResponse => NotFound(new ErrorDetails
                {
                    Message = ((NotFoundResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status404NotFound
                }),
                BadRequestResponse => BadRequest(new ErrorDetails
                {
                    Message = ((BadRequestResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status400BadRequest
                }),

                _ => throw new NotImplementedException()
            };
        }
    }
}