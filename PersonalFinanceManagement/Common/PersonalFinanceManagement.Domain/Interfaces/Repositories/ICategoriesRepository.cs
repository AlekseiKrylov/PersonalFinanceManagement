using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repositories
{
    public interface ICategoriesRepository : IRepository<Category>
    {
        Task<int> GetCountInWalletAsync(int walletId, CancellationToken cancel = default);
        Task<IEnumerable<Category>> GetAllInWalletAsync(int walletId, CancellationToken cancel = default);
        Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId = null, CancellationToken cancel = default);
        Task<IPage<Category>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default);
    }
}
