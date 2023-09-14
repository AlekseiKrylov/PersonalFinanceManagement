using MudBlazor;
using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class ResetPasswordViewModel : ComponentBase
    {
        protected string _resetPasswordToken;
        protected UserRegistrationAndRestoration _resetReqest;
        protected ResetPasswordRequest _resetReqestModel = new();

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] IDialogService DialogService { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task ResetAsync()
        {
            _resetPasswordToken = _resetReqestModel.ResetPasswordToken;
            _resetReqest = new()
            {
                Email = _resetReqestModel.Email,
                Password = _resetReqestModel.Password,
                ConfirmPassword = _resetReqestModel.ConfirmPassword
            };


            var response = await UsersWebApiClient.ResetPasswordAsync(_resetReqest, _resetPasswordToken);
            //await ResultDialog(response is null ? "User not found"
            //                 : response is true ? "Password successfully changed"
            //                 : "Wrong reset code");

            if (response is true)
                NavigationManager.NavigateTo("/login");
        }

        private async Task ResultDialog(string message)
        {
            await DialogService.ShowMessageBox("Information", message);
        }
    }
}
