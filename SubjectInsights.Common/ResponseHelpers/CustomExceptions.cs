using System;

namespace SubjectInsights.Common.ResponseHelpers
{
    public class CustomExceptions
    {
        public static CustomResponse<bool> GenerateExceptionForApp(Exception ex)
        {
            return new CustomResponse<bool>() { Code = StatusCode.Fail, Message = ex.Message, Data = false };
        }

        public static CustomResponse<bool> GenerateExceptionForApp(string message)
        {
            return new CustomResponse<bool>() { Code = StatusCode.Fail, Message = message, Data = false };
        }

        public static CustomResponse<bool> GenerateExceptionForApp(string message, string errorID)
        {
            return new CustomResponse<bool>() { Code = StatusCode.Fail, Message = message, Data = false };
        }
    }
}
