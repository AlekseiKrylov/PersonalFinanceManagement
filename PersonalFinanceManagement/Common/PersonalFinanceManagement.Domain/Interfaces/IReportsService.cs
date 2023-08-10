using PersonalFinanceManagement.Domain.BLLModels.Reports;

namespace PersonalFinanceManagement.Domain.Interfaces
{
    public interface IReportsService
    {
        Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel);
        Task<PeriodTransactionsReport> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel);
    }
}
