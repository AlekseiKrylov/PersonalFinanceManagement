using AutoMapper;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces;
using PersonalFinanceManagement.Domain.Interfaces.Repository;

namespace PersonalFinanceManagement.BLL.Services
{
    public class ReportsService : IReportsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public ReportsService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        protected virtual IEnumerable<TransactionWithCategory> GetDTO(IEnumerable<Transaction> items) => _mapper.Map<IEnumerable<TransactionWithCategory>>(items);

        public async Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel)
        {
            var transactions = await _transactionRepository.GetPeriodWithCategoryAsync(walletId, date, date, cancel).ConfigureAwait(false);
            decimal totalIncome = transactions.Where(t => t.Category.IsIncome).Sum(t => t.Amount);
            decimal totalExpenses = transactions.Where(t => !t.Category.IsIncome).Sum(t => t.Amount);

            var report = new DailyTransactionsReport
            {
                WalletId = walletId,
                Date = date,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Transactions = GetDTO(transactions.ToList()),
            };
            return report;
        }

        public async Task<PeriodTransactionsReport> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel)
        {
            var transactions = await _transactionRepository.GetPeriodWithCategoryAsync(walletId, startDate, endDate, cancel).ConfigureAwait(false);
            decimal totalIncome = transactions.Where(t => t.Category.IsIncome).Sum(t => t.Amount);
            decimal totalExpenses = transactions.Where(t => !t.Category.IsIncome).Sum(t => t.Amount);

            var report = new PeriodTransactionsReport
            {
                WalletId = walletId,
                StartDate = startDate,
                EndDate = endDate,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Transactions = GetDTO(transactions),
            };
            return report;
        }
    }
}
