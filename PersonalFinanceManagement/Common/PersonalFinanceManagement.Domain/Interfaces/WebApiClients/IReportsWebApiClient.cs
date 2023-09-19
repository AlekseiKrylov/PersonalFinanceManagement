using PersonalFinanceManagement.Domain.BLLModels.Reports;

namespace PersonalFinanceManagement.Domain.Interfaces.WebApiClients
{
    public interface IReportsWebApiClient
    {
        Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel = default);
        Task<PeriodTransactionsReport> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default);
    }
}
