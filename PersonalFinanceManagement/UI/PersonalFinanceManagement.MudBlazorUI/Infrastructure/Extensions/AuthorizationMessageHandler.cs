using Blazored.SessionStorage;
using PersonalFinanceManagement.Domain.UIModels;
using System.Net.Http.Headers;

namespace PersonalFinanceManagement.MudBlazorUI.Infrastructure.Extensions
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ISessionStorageService _sessionStorageService;

        public AuthorizationMessageHandler(ISessionStorageService sessionStorageService) => _sessionStorageService = sessionStorageService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancel)
        {
            var userSession = await _sessionStorageService.ReadEncryptedItemAsync<UserSession>("UserSession");
            if (userSession != null && userSession.ExpiryTimeStamp > DateTime.Now && !string.IsNullOrWhiteSpace(userSession.AccessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSession.AccessToken);

            return await base.SendAsync(request, cancel);
        }
    }
}