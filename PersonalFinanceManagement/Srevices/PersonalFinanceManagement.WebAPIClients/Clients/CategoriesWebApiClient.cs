using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class CategoriesWebApiClient : EntitiesWebApiClient<CategoryDTO, CategoryCreateDTO>, ICategoriesWebApiClient
    {
        private readonly HttpClient _httpClient;

        public CategoriesWebApiClient(HttpClient httpClient) : base(httpClient) => _httpClient = httpClient;

        public async Task<int> GetCountInWalletAsync(int walletId, CancellationToken cancel = default) =>
               await _httpClient.GetFromJsonAsync<int>($"in-wallet-count?walletId={walletId}", cancel);

        public async Task<IEnumerable<CategoryDTO>> GetAllInWalletAsync(int walletId, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>($"in-wallet?walletId={walletId}", cancel).ConfigureAwait(false);

        public async Task<IPage<CategoryDTO>> GetPageAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("page-with-restriction");
            urlBuilder.Append($"?pageIndex={pageIndex}");
            urlBuilder.Append($"&pageSize={pageSize}");

            if (walletId.HasValue)
                urlBuilder.Append($"&walletId={walletId}");

            var response = await _httpClient.GetAsync(urlBuilder.ToString(), cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new PageItems<CategoryDTO>(Enumerable.Empty<CategoryDTO>(), 0, pageIndex, pageSize);

            return await response.EnsureSuccessStatusCode().Content
                                 .ReadFromJsonAsync<PageItems<CategoryDTO>>(cancellationToken: cancel)
                                 .ConfigureAwait(false);
        }
    }
}
