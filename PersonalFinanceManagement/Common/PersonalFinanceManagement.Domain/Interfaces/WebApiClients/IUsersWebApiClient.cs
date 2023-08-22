using PersonalFinanceManagement.Domain.BLLModels;

namespace PersonalFinanceManagement.Domain.Interfaces.WebApiClients
{
    public interface IUsersWebApiClient
    {
        Task RegisterUserAsync(UserRegistrationAndRestoration userRegistration, CancellationToken cancel = default);
        Task<string> UserLoginAsync(UserLogin userLogin, CancellationToken cancel = default);
        Task<bool> VerifyUserAsync(string verificationToken, CancellationToken cancel = default);
        Task<bool> ForgotPasswordAsync(string email, CancellationToken cancel = default);
        Task<string> ResetPasswordAsync(UserRegistrationAndRestoration userRegistration, string token, CancellationToken cancel = default);
    }
}
