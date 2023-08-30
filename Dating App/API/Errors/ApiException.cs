namespace DatingApplication.Errors
{
    public class ApiException
    {

        public ApiException(int statusCode,string message, string detailes)
        {
            Message = message;
            StatusCode = statusCode;
            Details = detailes;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
