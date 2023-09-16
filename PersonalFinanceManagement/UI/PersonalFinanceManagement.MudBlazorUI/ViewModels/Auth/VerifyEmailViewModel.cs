using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class VerifyEmailViewModel : ComponentBase
    {
        [Required]
        protected string _emailVerificationCode;

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task<ApiResult<string>> EmailVerificationAsync()
        {
            var response = await UsersWebApiClient.VerifyUserAsync(_emailVerificationCode);
            if (!response.IsSuccessful)
                return response;

            NavigationManager.NavigateTo("/login");
            return response;
        }
    }
}
