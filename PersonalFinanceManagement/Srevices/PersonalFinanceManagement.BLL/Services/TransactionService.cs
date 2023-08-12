using AutoMapper;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services
{
    public class TransactionService : EntityServiceBase<TransactionDTO, TransactionCreateDTO, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(transactionRepository, mapper)
        {
            if (!int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("id")?.Value, out int userId) && userId <= 0)
                throw new UnauthorizedAccessException("User is not authorized.");
                
            transactionRepository.SetUserId(userId);

            _transactionRepository = transactionRepository;
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

        public override async Task<TransactionDTO> AddAsync(TransactionCreateDTO item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _transactionRepository.CheckEntitiesExistAsync(item.WalletId, item.CategoryId, null, cancel))
                throw new InvalidOperationException("Wallet or category not found.");

            return GetItem(await _transactionRepository.AddAsync(GetBase(item), cancel));
        }

        public override async Task<TransactionDTO> UpdateAsync(TransactionDTO item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if(!await _transactionRepository.CheckEntitiesExistAsync(item.WalletId, item.CategoryId, item.Id, cancel))
                throw new InvalidOperationException("Wallet, category or transaction not found.");

            return GetItem(await _transactionRepository.UpdateAsync(GetBase(item), cancel));
        }
    }
}
