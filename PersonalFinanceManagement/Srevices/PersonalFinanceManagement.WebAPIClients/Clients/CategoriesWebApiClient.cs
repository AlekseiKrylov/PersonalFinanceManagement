using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Repositories;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;
using System.Net.Http.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class CategoriesWebApiClient : EntitiesWebApiClient<CategoryDTO, CategoryCreateDTO>
    {
        private readonly HttpClient _httpClient;

        public CategoriesWebApiClient(HttpClient httpClient) : base(httpClient) => _httpClient = httpClient;

        public async Task<int> GetCountInWallet(int walletId, CancellationToken cancel = default) =>
               await _httpClient.GetFromJsonAsync<int>($"count-in-wallet[{walletId}]", cancel);

        public async Task<IEnumerable<CategoryDTO>> GetAllInWallet(int walletId, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>($"categories-in-wallet[{walletId}]", cancel).ConfigureAwait(false);

        public async Task<IPage<CategoryDTO>> GetPage(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default)
        {
            var url = $"page-with-restriction[{pageIndex}:{pageSize}]";

            if (walletId.HasValue)
                url += $"?walletId={walletId}";

            return await _httpClient.GetFromJsonAsync<IPage<CategoryDTO>>(url, cancel);
        }
    }
}
