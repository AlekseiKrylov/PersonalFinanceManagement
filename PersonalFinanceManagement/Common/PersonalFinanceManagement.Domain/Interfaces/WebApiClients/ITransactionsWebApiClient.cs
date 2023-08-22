using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.WebApiClients
{
    public interface ITransactionsWebApiClient
    {
        Task<bool> MoveTransactionsToAnotherCategory(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default);
        Task<int> GetCountInCategory(int categoryId, CancellationToken cancel = default);
        Task<IEnumerable<TransactionDTO>> GetAllInCategory(int categoryId, CancellationToken cancel = default);
        Task<IPage<TransactionDTO>> GetPage(int pageIndex, int pageSize, int? walletId = null, int? categoryId = null, CancellationToken cancel = default);
    }
}