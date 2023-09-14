using PersonalFinanceManagement.WebAPIClients.Exceptions;
using System.Net;
using System.Text.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients.Base
{
    public class WebApiClientBase
    {
        protected Exception HandleException(Exception exception)
        {
            return exception switch
            {
                JsonException jsonEx => new ApiDeserializationException("An error occurred while deserializing the API response.", jsonEx),
                HttpRequestException httpEx => new ApiException("An error occurred while executing the query.", httpEx.StatusCode ?? HttpStatusCode.InternalServerError),
                _ => new ApiException("An unknown error occurred.", HttpStatusCode.NotImplemented),
            };
        }
    }
}
