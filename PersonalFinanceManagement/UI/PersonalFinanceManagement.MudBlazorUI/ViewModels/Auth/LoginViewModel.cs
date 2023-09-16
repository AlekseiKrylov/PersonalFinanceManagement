using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.MudBlazorUI.Servises;
using System.IdentityModel.Tokens.Jwt;
using PersonalFinanceManagement.Domain.APIModels;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class LoginViewModel : ComponentBase
    {
        protected UserLogin _loginRequest = new();
        private readonly UserSession _userSession = new();

        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }
        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }

        protected async Task<ApiResult<string>> AuthenticateAsync()
        {
            var response = await UsersWebApiClient.UserLoginAsync(_loginRequest);
            if (!response.IsSuccessful)
                return response;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(response.Value);

            _userSession.AccessToken = response.Value;
            _userSession.UserId = int.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == "id").Value, out int id) ? id : throw new Exception("Error authentication");
            _userSession.Name = jwtToken.Claims.FirstOrDefault(c => c.Type == "email").Value;
            _userSession.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email").Value;
            _userSession.Role = "User";
            _userSession.ExpiryTimeStamp = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local);

            await ((CustomAuthStateProvider)AuthenticationStateProvider).UpdateAuthenticationStateAsync(_userSession);
            NavigationManager.NavigateTo("/", true);
            
            return new ApiResult<string>(value: string.Empty);
        }
    }
}
