using MudBlazor;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class VerifyEmailViewModel : ComponentBase
    {
        [Required]
        protected string _emailVerificationCode;

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] IDialogService DialogService { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task EmailVerificationAsync()
        {
            var response = await UsersWebApiClient.VerifyUserAsync(_emailVerificationCode);

            await ResultDialog(response ? "Email successfully verified" : "Wrong verify code");

            if (response)
                NavigationManager.NavigateTo("/login");
        }

        private async Task ResultDialog(string message)
        {
            await DialogService.ShowMessageBox("Information", message);
        }
    }
}
