using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using System.Net;
using System.Net.Http.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class UsersWebApiClient : IUsersWebApiClient
    {
        private readonly HttpClient _httpClient;

        public UsersWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task RegisterUserAsync(UserRegistrationAndRestoration userRegistration, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsJsonAsync("register", userRegistration, cancel);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> UserLoginAsync(UserLogin userLogin, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsJsonAsync("login", userLogin, cancel);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancel);
        }

        public async Task<bool> VerifyUserAsync(string verificationToken, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsync($"verify?verificationToken={verificationToken}", null, cancel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ForgotPasswordAsync(string email, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsync($"forgot-password?email={email}", null, cancel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool?> ResetPasswordAsync(UserRegistrationAndRestoration userRestoration, string token, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsJsonAsync($"reset-password?token={token}", userRestoration, cancel);
            
            if (response.IsSuccessStatusCode)
                return true;
            
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return false;
            
            return null;
        }
    }
}
