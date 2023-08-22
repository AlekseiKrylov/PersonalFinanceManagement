using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repositories
{
    public interface ICategoriesRepository : IRepository<Category>
    {
        Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId = null, CancellationToken cancel = default);
        Task<IPage<Category>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default);
    }
}
