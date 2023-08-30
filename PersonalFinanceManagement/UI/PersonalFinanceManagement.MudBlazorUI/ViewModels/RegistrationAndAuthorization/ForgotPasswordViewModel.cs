using Microsoft.AspNetCore.Components;
using MudBlazor;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.RegistrationAndAuthorization
{
    public class ForgotPasswordViewModel : ComponentBase
    {
        [Required, EmailAddress]
        protected string _userEmail;

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] IDialogService DialogService { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task ForgotPasswordAsync()
        {
            var response = await UsersWebApiClient.ForgotPasswordAsync(_userEmail);

            await ResultDialog(response ? "You may reset your password" : "User not found");

            if (response)
                NavigationManager.NavigateTo("/reset");
        }

        private async Task ResultDialog(string message)
        {
            await DialogService.ShowMessageBox("Information", message);
        }
    }
}
