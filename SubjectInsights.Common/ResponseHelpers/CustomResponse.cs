namespace SubjectInsights.Common.ResponseHelpers
{
    public class CustomResponse<TData>
    {
        public StatusCode Code { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
        public string ErrorMessage { get; set; }
        public long TotalCount { get; set; }



    }

    public enum StatusCode
    {
        Success = 200,
        Fail = 300,
        Exception = 500,
        Authorization = 403
    }

    public static class ErrorMessage
    {
        public const string NO_PERMISSION = "Unauthorized Access";
    }
}
