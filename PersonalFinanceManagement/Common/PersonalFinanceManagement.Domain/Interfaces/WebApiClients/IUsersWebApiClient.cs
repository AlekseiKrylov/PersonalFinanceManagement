using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.BLLModels;

namespace PersonalFinanceManagement.Domain.Interfaces.WebApiClients
{
    public interface IUsersWebApiClient
    {
        Task<ApiResult<string>> RegisterUserAsync(UserRegistrationAndRestoration userRegistration, CancellationToken cancel = default);
        Task<ApiResult<string>> UserLoginAsync(UserLogin userLogin, CancellationToken cancel = default);
        Task<ApiResult<string>> VerifyUserAsync(string verificationToken, CancellationToken cancel = default);
        Task<ApiResult<string>> ForgotPasswordAsync(string email, CancellationToken cancel = default);
        Task<ApiResult<string>> ResetPasswordAsync(UserRegistrationAndRestoration userResetPassword, string token, CancellationToken cancel = default);
    }
}
