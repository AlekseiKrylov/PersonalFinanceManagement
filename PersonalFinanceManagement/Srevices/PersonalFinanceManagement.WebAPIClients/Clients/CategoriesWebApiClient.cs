using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    internal class CategoriesWebApiClient : EntitiesWebApiClient<CategoryDTO, CategoryCreateDTO>
    {
        public CategoriesWebApiClient(HttpClient httpClient) : base(httpClient) { }
    }
}
