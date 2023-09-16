using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class UsersWebApiClient : WebApiClientBase, IUsersWebApiClient
    {
        private readonly HttpClient _httpClient;

        public UsersWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<ApiResult<string>> RegisterUserAsync(UserRegistrationAndRestoration userRegistration, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("register", userRegistration, cancel);
                var content = await response.Content.ReadAsStringAsync(cancel);

                if (response.IsSuccessStatusCode)
                    return new ApiResult<string>(value: content);

                var error = JsonSerializer.Deserialize<ApiError>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResult<string>(error.Message);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<ApiResult<string>> UserLoginAsync(UserLogin userLogin, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("login", userLogin, cancel);
                var content = await response.Content.ReadAsStringAsync(cancel);

                if (response.IsSuccessStatusCode)
                    return new ApiResult<string>(value: content);

                var error = JsonSerializer.Deserialize<ApiError>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return error?.StatusCode switch
                {
                    (int)HttpStatusCode.NotFound => new ApiResult<string>(error.Message),
                    (int)HttpStatusCode.Forbidden => new ApiResult<string>(ApiResultStatus.Partial, error.Message),
                    (int)HttpStatusCode.Unauthorized => new ApiResult<string>(ApiResultStatus.Partial, error.Message),
                    _ => new ApiResult<string>(error?.Message ?? "An unknown error occurred.")
                };
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<ApiResult<string>> VerifyUserAsync(string verificationToken, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsync($"verify?verificationToken={verificationToken}", null, cancel);
                var content = await response.Content.ReadAsStringAsync(cancel);

                if (response.IsSuccessStatusCode)
                    return new ApiResult<string>(value: content);

                var error = JsonSerializer.Deserialize<ApiError>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResult<string>(error?.Message ?? "An unknown error occurred.");
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<ApiResult<string>> ForgotPasswordAsync(string email, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsync($"forgot-password?email={email}", null, cancel);
                var content = await response.Content.ReadAsStringAsync(cancel);

                if (response.IsSuccessStatusCode)
                    return new ApiResult<string>(value: content);

                var error = JsonSerializer.Deserialize<ApiError>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResult<string>(error?.Message ?? "An unknown error occurred.");
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<ApiResult<string>> ResetPasswordAsync(UserRegistrationAndRestoration userRestoration, string token, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"reset-password?token={token}", userRestoration, cancel);
                var content = await response.Content.ReadAsStringAsync(cancel);

                if (response.IsSuccessStatusCode)
                    return new ApiResult<string>(value: content);

                var error = JsonSerializer.Deserialize<ApiError>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return error?.StatusCode switch
                {
                    (int)HttpStatusCode.Forbidden => new ApiResult<string>(ApiResultStatus.Partial, error.Message),
                    _ => new ApiResult<string>(error?.Message ?? "An unknown error occurred.")
                };
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }
    }
}
