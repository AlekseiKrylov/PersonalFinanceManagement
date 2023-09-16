using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.APIModels;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Domain.UIModels;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class ResetPasswordViewModel : ComponentBase
    {
        protected string _resetPasswordToken;
        protected UserRegistrationAndRestoration _resetReqest;
        protected ResetPasswordRequest _resetReqestModel = new();

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task<ApiResult<string>> ResetAsync()
        {
            _resetPasswordToken = _resetReqestModel.ResetPasswordToken;
            _resetReqest = new()
            {
                Email = _resetReqestModel.Email,
                Password = _resetReqestModel.Password,
                ConfirmPassword = _resetReqestModel.ConfirmPassword
            };


            var response = await UsersWebApiClient.ResetPasswordAsync(_resetReqest, _resetPasswordToken);
            if (!response.IsSuccessful)
                return response;

            NavigationManager.NavigateTo("/login");
            return response;
        }
    }
}
