using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.MudBlazorUI.Servises;
using System.IdentityModel.Tokens.Jwt;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.RegistrationAndAuthorization
{
    public class LoginViewModel : ComponentBase
    {
        protected UserLogin _loginRequest = new();
        private readonly UserSession _userSession = new();

        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }
        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }

        protected async Task AuthenticateAsync()
        {
            var loginResponse = await UsersWebApiClient.UserLoginAsync(_loginRequest);
            if (string.IsNullOrWhiteSpace(loginResponse))
                return;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(loginResponse);

            _userSession.AccessToken = loginResponse;
            _userSession.Name = jwtToken.Claims.FirstOrDefault(c => c.Type == "email").Value;
            _userSession.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email").Value;
            _userSession.Role = "User";
            _userSession.ExpiryTimeStamp = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local);

            await ((CustomAuthStateProvider)AuthenticationStateProvider).UpdateAuthenticationStateAsync(_userSession);
            NavigationManager.NavigateTo("/", true);
        }
    }
}
