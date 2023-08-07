using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsWithCategoryAsync(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default);
        
        Task<bool> MoveTransactionsToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default);
        
        Task<int> GetTransactionCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default);
    }
}
