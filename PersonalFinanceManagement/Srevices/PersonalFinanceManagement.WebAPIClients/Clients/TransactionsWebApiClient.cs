using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;
using System.Net;
using System.Net.Http.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class TransactionsWebApiClient : EntitiesWebApiClient<TransactionDTO, TransactionCreateDTO>, ITransactionsWebApiClient
    {
        private readonly HttpClient _httpClient;

        public TransactionsWebApiClient(HttpClient httpClient) : base(httpClient) => _httpClient = httpClient;

        public async Task<bool> MoveTransactionsToAnotherCategory(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsync($"move[{walletId}:{sourceCategoryId}:{targetCategoryId}]", null, cancel).ConfigureAwait(false);
            return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
        }

        public async Task<int> GetCountInCategory(int categoryId, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<int>($"count-in-category[{categoryId}]", cancel);

        public async Task<IEnumerable<TransactionDTO>> GetAllInCategory(int categoryId, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<TransactionDTO>>($"transactions-in-category[{categoryId}]", cancel).ConfigureAwait(false);

        public async Task<IPage<TransactionDTO>> GetPageAsync(int pageIndex, int pageSize, int? walletId = null, int? categoryId = null, CancellationToken cancel = default)
        {
            var url = $"page-with-restriction[{pageIndex}:{pageSize}]";

            if (walletId.HasValue)
                url += $"?walletId={walletId}";

            if (walletId.HasValue && categoryId.HasValue)
                url += $"&categoryId={categoryId}";

            if (!walletId.HasValue && categoryId.HasValue)
                url += $"?categoryId={categoryId}";

            var response = await _httpClient.GetAsync(url, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new PageItems<TransactionDTO>(Enumerable.Empty<TransactionDTO>(), 0, pageIndex, pageSize);

            return await response.EnsureSuccessStatusCode().Content
                                 .ReadFromJsonAsync<PageItems<TransactionDTO>>(cancellationToken: cancel)
                                 .ConfigureAwait(false);
        }
    }
}
