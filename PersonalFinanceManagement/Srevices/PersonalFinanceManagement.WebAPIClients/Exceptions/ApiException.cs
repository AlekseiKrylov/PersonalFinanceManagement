using System.Net;

namespace PersonalFinanceManagement.WebAPIClients.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(string message, HttpStatusCode statusCode, Exception exception)
            : base(message, exception) => StatusCode = statusCode;

        public ApiException(string message, HttpStatusCode statusCode)
            : base(message) => StatusCode = statusCode;
    }
}
