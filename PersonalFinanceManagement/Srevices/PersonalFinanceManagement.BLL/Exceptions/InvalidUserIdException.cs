namespace PersonalFinanceManagement.BLL.Exceptions
{
    public class InvalidUserIdException : Exception
    {
        public InvalidUserIdException(string message) : base(message) { }
    }
}
