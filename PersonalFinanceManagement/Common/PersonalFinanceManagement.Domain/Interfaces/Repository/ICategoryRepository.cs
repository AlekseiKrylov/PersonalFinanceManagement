using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId = null, CancellationToken cancel = default);
        Task<IPage<Category>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default);
    }
}
