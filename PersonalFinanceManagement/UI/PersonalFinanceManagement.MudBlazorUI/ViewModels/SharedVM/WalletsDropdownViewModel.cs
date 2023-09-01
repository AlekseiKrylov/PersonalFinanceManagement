using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels.SharedVM
{
    public class WalletsDropdownViewModel : ComponentBase
    {
        protected List<WalletDTO> _wallets = new();

        [Inject] IEntitiesWebApiClient<WalletDTO, WalletCreateDTO> WalletsWebAtiClient { get; init; }
        [Parameter] public WalletDTO SelectedWallet { get; set; }
        [Parameter] public Func<WalletDTO, Task> OnWalletChamged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _wallets = (await WalletsWebAtiClient.GetAllAsync()).ToList();
        }

        protected async Task WalletChanged(WalletDTO wallet)
        {
            SelectedWallet = wallet;
            await OnWalletChamged(wallet);
        }
    }
}
