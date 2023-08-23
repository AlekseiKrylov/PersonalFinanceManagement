using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Repositories;
using PersonalFinanceManagement.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.Domain.Interfaces.WebApiClients
{
    public interface ICategoriesWebApiClient : IEntitiesWebApiClient<CategoryDTO, CategoryCreateDTO>
    {
        Task<int> GetCountInWallet(int walletId, CancellationToken cancel = default);
        Task<IEnumerable<CategoryDTO>> GetAllInWallet(int walletId, CancellationToken cancel = default);
        Task<IPage<CategoryDTO>> GetPage(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default);
    }
}
