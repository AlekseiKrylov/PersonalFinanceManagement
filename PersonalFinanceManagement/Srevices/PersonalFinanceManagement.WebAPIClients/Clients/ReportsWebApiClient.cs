using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using System.Net.Http.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class ReportsWebApiClient : IReportsWebApiClient
    {
        private readonly HttpClient _httpClient;

        public ReportsWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<DailyTransactionsReport>($"daily/wallet/{walletId}/{date:yyyy-MM-dd}", cancel)
                .ConfigureAwait(false);

        public async Task<PeriodTransactionsReport> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<PeriodTransactionsReport>($"period/wallet/{walletId}/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}", cancel)
                .ConfigureAwait(false);
    }
}
