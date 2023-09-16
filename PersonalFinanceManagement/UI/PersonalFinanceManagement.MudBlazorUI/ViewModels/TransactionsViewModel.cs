using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels
{
    public class TransactionsViewModel : ComponentBase
    {
        protected IEnumerable<WalletDTO> _wallets = new List<WalletDTO>();
        protected IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();
        protected List<TransactionDTO> _transactions = new List<TransactionDTO>();

        [Inject] IEntitiesWebApiClient<WalletDTO, WalletCreateDTO> WalletsWebApiClient { get; init; }
        [Inject] ICategoriesWebApiClient CategoriesWebApiClient { get; init; }
        [Inject] ITransactionsWebApiClient TransactionsWebApiClient { get; init; }

        protected async Task GetWalletsAsync()
        {
            _wallets = await WalletsWebApiClient.GetAllAsync().ConfigureAwait(false);
        }

        protected async Task GetCatigoriesAsync()
        {
            _categories = await CategoriesWebApiClient.GetAllAsync().ConfigureAwait(false);
        }

        protected async Task GetTransactionAsync()
        {
            _transactions = (await TransactionsWebApiClient.GetAllAsync().ConfigureAwait(false)).ToList();
        }

        protected async Task<TransactionDTO> UpdateTransactionAsync(TransactionWithCategory transaction)
        {
            var updatedTransaction = new TransactionDTO
            {
                Id = transaction.Id,
                Name = transaction.Name,
                Note = transaction.Note,
                Amount = transaction.Amount,
                Date = (DateTime)transaction.Date,
                CategoryId = transaction.Category.Id,
            };

            return await TransactionsWebApiClient.UpdateAsync(updatedTransaction);
        }

        protected async Task<TransactionDTO> RemoveTransactionAsync(int id)
        {
            return await TransactionsWebApiClient.DeleteByIdAsync(id);
        }

        protected async Task<PageItems<TransactionDTO>> GetPageAsync(int pageIndex, int pageSize, int? walletId = null, int? categoryId = null)
        {
            var response = await TransactionsWebApiClient.GetPageAsync(pageIndex, pageSize, walletId, categoryId).ConfigureAwait(false);
            return (PageItems<TransactionDTO>)response;
        }
    }
}
