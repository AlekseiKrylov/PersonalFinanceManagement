using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.Auth
{
    public class RegisterViewModel : ComponentBase
    {
        protected UserRegistrationAndRestoration _registerReqest = new();

        [Inject] IUsersWebApiClient UsersWebApiClient { get; init; }
        [Inject] NavigationManager NavigationManager { get; init; }

        protected async Task RegisterAsync()
        {
            await UsersWebApiClient.RegisterUserAsync(_registerReqest);
            NavigationManager.NavigateTo("/verify");
        }
    }
}
