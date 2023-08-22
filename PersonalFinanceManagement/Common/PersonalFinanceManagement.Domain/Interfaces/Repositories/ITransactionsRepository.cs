using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repositories
{
    public interface ITransactionsRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetPeriodWithCategoryAsync(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default);
        Task<bool> MoveToAnotherCategoryOfSameWalletAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default);
        Task<int> GetCountInCategoryAsync(int categoryId, CancellationToken cancel = default);
        Task<IEnumerable<Transaction>> GetAllInCategoryAsync(int categoryId, CancellationToken cancel = default);
        Task<bool> CheckEntitiesExistAsync(int categoryId, int? transactionId = null, CancellationToken cancel = default);
        Task<IPage<Transaction>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, int? categoryId = null, CancellationToken cancel = default);
    }
}
