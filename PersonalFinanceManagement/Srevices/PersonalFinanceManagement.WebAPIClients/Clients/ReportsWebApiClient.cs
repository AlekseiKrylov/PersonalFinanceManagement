using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;
using System.Net.Http.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class ReportsWebApiClient : WebApiClientBase, IReportsWebApiClient
    {
        private readonly HttpClient _httpClient;

        public ReportsWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<DailyTransactionsReport> GetDailyReport(int walletId, DateTime date, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<DailyTransactionsReport>($"daily[{walletId}:{date.ToString("yyyy-MM-dd")}]", cancel).ConfigureAwait(false);

        public async Task<PeriodTransactionsReport> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<PeriodTransactionsReport>($"period[{walletId}:{startDate.ToString("yyyy-MM-dd")}:{endDate.ToString("yyyy-MM-dd")}]", cancel).ConfigureAwait(false);
    }
}
