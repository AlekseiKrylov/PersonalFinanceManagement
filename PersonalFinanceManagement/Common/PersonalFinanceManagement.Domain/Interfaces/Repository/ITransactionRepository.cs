using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetWithCategoryAsync(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default);
        Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default);
        Task<int> GetCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default);
        Task<IEnumerable<Transaction>> GetAllInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default);
        void SetUserId(int userId);
        Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId = null, int? transactionId = null, CancellationToken cancel = default);
    }
}
