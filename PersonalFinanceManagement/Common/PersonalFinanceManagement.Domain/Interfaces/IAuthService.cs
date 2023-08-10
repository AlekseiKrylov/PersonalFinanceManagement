using PersonalFinanceManagement.Domain.BLLModels;

namespace PersonalFinanceManagement.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<JWToken> UserLogin(string email, string password, CancellationToken cancel = default);
    }
}
