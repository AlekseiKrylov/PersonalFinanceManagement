﻿using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.ModelsDTO;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.Domain.Interfaces.Services
{
    public interface ITransactionService : IEntityService<TransactionDTO, Transaction>
    {
        Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default);
        Task<int> GetCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default);
        Task<IEnumerable<TransactionDTO>> GetAllInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default);
    }
}
