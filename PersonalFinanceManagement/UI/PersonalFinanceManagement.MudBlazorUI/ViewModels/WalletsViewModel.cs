using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels
{
    public class WalletsViewModel : ComponentBase
    {
        protected IEnumerable<WalletDTO> _wallets;
        [Inject] IEntitiesWebApiClient<WalletDTO, WalletCreateDTO> WalletsWebApiClient { get; init; }
        
        protected override async Task OnInitializedAsync()
        {
            _wallets = (await WalletsWebApiClient.GetAllAsync()).ToList();
        }
    }
}
