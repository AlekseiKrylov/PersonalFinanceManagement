namespace PersonalFinanceManagement.WebAPIClients.Exceptions
{
    public class ApiDeserializationException : Exception
    {
        public ApiDeserializationException(string message)
            : base(message) { }
        public ApiDeserializationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
