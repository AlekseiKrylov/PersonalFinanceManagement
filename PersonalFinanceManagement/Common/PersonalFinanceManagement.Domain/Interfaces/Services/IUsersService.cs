using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;

namespace PersonalFinanceManagement.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task<bool> ExistByEmailAsync(string email, CancellationToken cancel = default);
        Task<UserDTO> RegisterUserAsync(string email, string password, CancellationToken cancel = default);
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancel = default);
        Task<User> GetUserByVerificationTokenAsync(string verificationToken, CancellationToken cancel = default);
        Task<bool> VerifyUserAsync(string verificationToken, CancellationToken cancel = default);
        Task<bool> ForgotPasswordAsync(string email, CancellationToken cancel = default);
        Task<bool?> ResetPasswordAsync(string email, string password, string resetToken, CancellationToken cancel = default);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
