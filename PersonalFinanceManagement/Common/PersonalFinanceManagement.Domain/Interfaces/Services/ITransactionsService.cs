using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.Domain.Interfaces.Services
{
    public interface ITransactionsService : IEntityService<TransactionDTO, TransactionCreateDTO, Transaction>
    {
        Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default);
        Task<int> GetCountInCategoryAsync(int categoryId, CancellationToken cancel = default);
        Task<IEnumerable<TransactionDTO>> GetAllInCategoryAsync(int categoryId, CancellationToken cancel = default);
        Task<IPage<TransactionDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, int? categoryId = null, CancellationToken cancel = default);
    }
}
