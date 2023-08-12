namespace PersonalFinanceManagement.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<string> UserLogin(string email, string password, CancellationToken cancel = default);
    }
}
