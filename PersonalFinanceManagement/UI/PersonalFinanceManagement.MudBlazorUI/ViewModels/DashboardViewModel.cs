using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels
{
    public class DashboardViewModel : ComponentBase
    {
        protected decimal _reportIncome;
        protected decimal _reportExpenses;
        protected IEnumerable<WalletDTO> _wallets = new List<WalletDTO>();
        protected IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();
        protected IEnumerable<TransactionWithCategory> _transactions = new List<TransactionWithCategory>();
        protected PeriodTransactionsReport _periodTransactionsReport = new();

        [Inject] IReportsWebApiClient ReportsWebApiClient { get; init; }
        [Inject] IEntitiesWebApiClient<WalletDTO, WalletCreateDTO> WalletsWebApiClient { get; init; }
        [Inject] ICategoriesWebApiClient CategoriesWebApiClient { get; init; }
        [Inject] ITransactionsWebApiClient TransactionsWebApiClient { get; init; }

        protected async Task GetWalletsAsync()
        {
            _wallets = await WalletsWebApiClient.GetAllAsync();
        }

        protected async Task GetCatigoriesInWallet(int walletId)
        {
            _categories = await CategoriesWebApiClient.GetAllInWalletAsync(walletId);
        }

        protected async Task GetPeriodReportAsync(int walletId, DateTime startDate, DateTime endDate)
        {
            ClearData();
            _periodTransactionsReport = await ReportsWebApiClient.GetPeriodReport(walletId, startDate, endDate);
            SetData(_periodTransactionsReport);
        }

        protected async Task<TransactionDTO> AddTransactionAsync(TransactionCreateDTO transaction)
        {
            return await TransactionsWebApiClient.AddAsync(transaction);
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

        private void ClearData()
        {
            _reportIncome = default;
            _reportExpenses = default;
            _transactions = new List<TransactionWithCategory>();
        }

        private void SetData(TransactionsReport report)
        {
            _reportIncome = report.TotalIncome;
            _reportExpenses = report.TotalExpenses;
            _transactions = report.Transactions.ToList();
        }
    }
}
