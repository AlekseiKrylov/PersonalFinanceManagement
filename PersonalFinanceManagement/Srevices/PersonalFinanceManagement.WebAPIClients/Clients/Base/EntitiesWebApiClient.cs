using PersonalFinanceManagement.Domain.UIModels;
using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.Interfaces.Entities;
using PersonalFinanceManagement.Interfaces.WebApiClients;
using System.Net;
using System.Net.Http.Json;

namespace PersonalFinanceManagement.WebAPIClients.Clients.Base
{
    public class EntitiesWebApiClient<T, TCreate> : IEntitiesWebApiClient<T, TCreate>
        where T : IEntity
        where TCreate : IEntity
    {
        private readonly HttpClient _httpClient;

        public EntitiesWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<bool> ExistIdAsync(int id, CancellationToken cancel = default)
        {
            var response = await _httpClient.GetAsync($"exists/{id}", cancel).ConfigureAwait(false);
            return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
        }

        public async Task<bool> ExistAsync(T item, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsJsonAsync("exists", item, cancel).ConfigureAwait(false);
            return response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode;
        }

        public async Task<int> GetCountAsync(CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<int>("count", cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<T>>("", cancel).ConfigureAwait(false);

        public async Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<T>>($"items?skip={skip}&count={count}", cancel).ConfigureAwait(false);

        public async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var response = await _httpClient.GetAsync($"page?pageIndex={pageIndex}&pageSize={pageSize}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new PageItems<T>(Enumerable.Empty<T>(), 0, pageIndex, pageSize);

            return await response.EnsureSuccessStatusCode().Content
                                 .ReadFromJsonAsync<PageItems<T>>(cancellationToken: cancel)
                                 .ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancel = default)
        {
            var response = await _httpClient.GetAsync($"{id}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response.EnsureSuccessStatusCode().Content
                                 .ReadFromJsonAsync<T>(cancellationToken: cancel)
                                 .ConfigureAwait(false);
        }

        public async Task<T> AddAsync(TCreate item, CancellationToken cancel = default)
        {
            var response = await _httpClient.PostAsJsonAsync("", item, cancel).ConfigureAwait(false);

            return await response.EnsureSuccessStatusCode().Content
                                 .ReadFromJsonAsync<T>(cancellationToken: cancel)
                                 .ConfigureAwait(false);
        }

        public async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            var response = await _httpClient.PutAsJsonAsync("", item, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response.EnsureSuccessStatusCode().Content
                                 .ReadFromJsonAsync<T>(cancellationToken: cancel)
                                 .ConfigureAwait(false);
        }

        public async Task<T> DeleteAsync(T item, CancellationToken cancel = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "")
            {
                Content = JsonContent.Create(item)
            };

            var response = await _httpClient.SendAsync(request, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response.EnsureSuccessStatusCode().Content
                                       .ReadFromJsonAsync<T>(cancellationToken: cancel)
                                       .ConfigureAwait(false);
        }

        public async Task<T> DeleteByIdAsync(int id, CancellationToken cancel = default)
        {
            var response = await _httpClient.DeleteAsync($"{id}", cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return default;

            return await response.EnsureSuccessStatusCode().Content
                                       .ReadFromJsonAsync<T>(cancellationToken: cancel)
                                       .ConfigureAwait(false);
        }
    }
}
