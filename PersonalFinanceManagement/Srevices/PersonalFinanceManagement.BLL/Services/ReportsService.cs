using PersonalFinanceManagement.Domain.DTOModels.Reports;
using PersonalFinanceManagement.Domain.Interfaces;

namespace PersonalFinanceManagement.BLL.Services
{
    public class ReportsService : IReportsService
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReportsService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel)
        {
            var transactions = await _transactionRepository.GetTransactionsAsync(walletId, date, date, cancel).ConfigureAwait(false);
            decimal totalIncome = transactions.Where(t => t.Category.IsIncome).Sum(t => t.Amount);
            decimal totalExpenses = transactions.Where(t => !t.Category.IsIncome).Sum(t => t.Amount);

            var report = new DailyTransactionsReport
            {
                WalletId = walletId,
                Date = date,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Transactions = transactions.ToList(),
            };
            return report;
        }

        public async Task<PeriodTransactionsReport> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel)
        {
            var transactions = await _transactionRepository.GetTransactionsAsync(walletId, startDate, endDate, cancel).ConfigureAwait(false);
            decimal totalIncome = transactions.Where(t => t.Category.IsIncome).Sum(t => t.Amount);
            decimal totalExpenses = transactions.Where(t => !t.Category.IsIncome).Sum(t => t.Amount);

            var report = new PeriodTransactionsReport
            {
                WalletId = walletId,
                StartDate = startDate,
                EndDate = endDate,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Transactions = transactions.ToList(),
            };
            return report;
        }
    }
}
