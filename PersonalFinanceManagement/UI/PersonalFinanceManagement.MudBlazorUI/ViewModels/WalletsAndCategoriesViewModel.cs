using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Interfaces.WebApiClients;
using PersonalFinanceManagement.MudBlazorUI.Infrastructure.Extensions;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels
{
    public class WalletsAndCategoriesViewModel : ComponentBase
    {
        protected List<WalletDTO> _wallets = new();
        protected List<CategoryDTO> _categoriesSelectedWallet;

        [Inject] IEntitiesWebApiClient<WalletDTO, WalletCreateDTO> WalletsWebApiClient { get; init; }
        [Inject] ICategoriesWebApiClient CategoriesWebApiClient { get; init; }
        [Inject] ITransactionsWebApiClient TransactionsWebApiClient { get; init; }
        [Inject] ISessionStorageService SessionStorageService { get; init; }


        protected override async Task OnInitializedAsync()
        {
            _wallets = (await WalletsWebApiClient.GetAllAsync().ConfigureAwait(false)).ToList();
        }

        protected async Task GetWalletCategoriesAsync(int id)
        {
            _categoriesSelectedWallet = (await CategoriesWebApiClient.GetAllInWalletAsync(id).ConfigureAwait(false)).ToList();
        }

        protected async Task<WalletDTO> AddWalletAsync(WalletDTO wallet)
        {
            var userSession = await SessionStorageService.ReadEncryptedItemAsync<UserSession>("UserSession");
            var newWallet = new WalletCreateDTO
            {
                Name = wallet.Name,
                Description = wallet.Description,
                UserId = userSession.UserId,
            };

            return await WalletsWebApiClient.AddAsync(newWallet).ConfigureAwait(false);
        }

        protected async Task<WalletDTO> UpdateWalletAsync(WalletDTO wallet)
        {
            return await WalletsWebApiClient.UpdateAsync(wallet).ConfigureAwait(false);
        }

        protected async Task<WalletDTO> DeleteWalletAsync(WalletDTO wallet)
        {
            return await WalletsWebApiClient.DeleteByIdAsync(wallet.Id).ConfigureAwait(false);
        }

        protected async Task<CategoryDTO> AddCategoryAsync(CategoryCreateDTO newCategory)
        {
            return await CategoriesWebApiClient.AddAsync(newCategory).ConfigureAwait(false);
        }

        protected async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO category)
        {
            return await CategoriesWebApiClient.UpdateAsync(category).ConfigureAwait(false);
        }

        protected async Task DeleteCategoryAsync(CategoryDTO category)
        {
            var deletedWallet = await CategoriesWebApiClient.DeleteByIdAsync(category.Id).ConfigureAwait(false);
        }

        protected async Task MoveTransactioncAndDeleteCategory(int walletId, int sourceCategoryId, int targetCategoryId)
        {
            var movingResult = await TransactionsWebApiClient
                .MoveTransactionsToAnotherCategory(walletId, sourceCategoryId, targetCategoryId).ConfigureAwait(false);

            if (movingResult)
                await CategoriesWebApiClient.DeleteByIdAsync(sourceCategoryId).ConfigureAwait(false);
        }
    }
}
