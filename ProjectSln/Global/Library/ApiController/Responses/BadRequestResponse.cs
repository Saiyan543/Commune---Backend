﻿namespace Main.Global.Library.ApiController.Responses
{
    public class BadRequestResponse : BaseResponse
    {
        public string Message { get; set; }

        public BadRequestResponse(string message) : base(false)
        {
            Message = message;
        }
    }
}