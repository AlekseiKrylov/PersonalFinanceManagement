namespace PersonalFinanceManagement.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> UserLogin(string email, string password, CancellationToken cancel = default);
    }
}
