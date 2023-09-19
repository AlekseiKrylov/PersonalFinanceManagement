using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class ForgotPasswordViewModel : ComponentBase
    {
        [Required, EmailAddress]
        protected string _userEmail;

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task<ApiResult<string>> ForgotPasswordAsync()
        {
            var response = await UsersWebApiClient.ForgotPasswordAsync(_userEmail);
            if (!response.IsSuccessful)
                return response;

            NavigationManager.NavigateTo("/reset");
            return response;
        }
    }
}
