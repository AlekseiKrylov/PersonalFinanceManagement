using AutoMapper;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repositories;
using PersonalFinanceManagement.Domain.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services
{
    public class ReportsService : IReportsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public ReportsService(ITransactionsRepository transactionsRepository, IMapper mapper)
        {
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
        }

        protected virtual IEnumerable<TransactionWithCategory> GetDTO(IEnumerable<Transaction> items) => _mapper.Map<IEnumerable<TransactionWithCategory>>(items);

        public async Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel)
        {
            var transactions = await _transactionsRepository.GetPeriodWithCategoryAsync(walletId, date, date, cancel).ConfigureAwait(false);
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
            var transactions = await _transactionsRepository.GetPeriodWithCategoryAsync(walletId, startDate, endDate, cancel).ConfigureAwait(false);
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
