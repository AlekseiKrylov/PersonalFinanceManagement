using AutoMapper;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Domain.ModelsDTO;

namespace PersonalFinanceManagement.BLL.Services
{
    public class TransactionService : EntityServiceBase<TransactionDTO, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(transactionRepository, mapper)
        {
            _transactionRepository = transactionRepository;

            if (int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("id")?.Value, out int userId) && userId > 0)
                transactionRepository.SetUserId(userId);
            else
                throw new UnauthorizedAccessException("User is not authorized.");
        }

        public async Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default)
        {
            return await _transactionRepository.MoveToAnotherCategoryAsync(walletId, sourceCategoryId, targetCategoryId, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default)
        {
            return await _transactionRepository.GetCountInCategoryAsync(walletId, categoryId, cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default)
        {
            return GetItem(await _transactionRepository.GetAllInCategoryAsync(walletId, categoryId, cancel).ConfigureAwait(false));
        }
    }
}
