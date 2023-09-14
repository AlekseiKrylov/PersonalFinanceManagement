using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;
using PersonalFinanceManagement.WebAPIClients.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class UsersWebApiClient : WebApiClientBase, IUsersWebApiClient
    {
        private readonly HttpClient _httpClient;

        public UsersWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<string> RegisterUserAsync(UserRegistrationAndRestoration userRegistration, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("register", userRegistration, cancel);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync(cancel);

                var errorContent = await response.Content.ReadAsStringAsync(cancel);
                var error = JsonSerializer.Deserialize<ApiError>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(error.Message, response.StatusCode);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<string> UserLoginAsync(UserLogin userLogin, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("login", userLogin, cancel);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync(cancel);

                var errorContent = await response.Content.ReadAsStringAsync(cancel);
                var error = JsonSerializer.Deserialize<ApiError>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(error.Message, response.StatusCode);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<bool> VerifyUserAsync(string verificationToken, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsync($"verify?verificationToken={verificationToken}", null, cancel);
                if (response.IsSuccessStatusCode)
                    return true;

                var errorContent = await response.Content.ReadAsStringAsync(cancel);
                var error = JsonSerializer.Deserialize<ApiError>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(error.Message, response.StatusCode);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<bool> ForgotPasswordAsync(string email, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsync($"forgot-password?email={email}", null, cancel);
                if (response.IsSuccessStatusCode)
                    return true;

                var errorContent = await response.Content.ReadAsStringAsync(cancel);
                var error = JsonSerializer.Deserialize<ApiError>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(error.Message, response.StatusCode);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<bool> ResetPasswordAsync(UserRegistrationAndRestoration userRestoration, string token, CancellationToken cancel = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"reset-password?token={token}", userRestoration, cancel);
                if (response.IsSuccessStatusCode)
                    return true;

                var errorContent = await response.Content.ReadAsStringAsync(cancel);
                var error = JsonSerializer.Deserialize<ApiError>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(error.Message, response.StatusCode);
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }
    }
}
