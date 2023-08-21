using AutoMapper;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.BLL.Services
{
    public class TransactionService : EntityServiceBase<TransactionDTO, TransactionCreateDTO, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper) : base(transactionRepository, mapper)
            => _transactionRepository = transactionRepository;

        public async Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel)
        {
            return await _transactionRepository.MoveToAnotherCategoryOfSameWalletAsync(walletId, sourceCategoryId, targetCategoryId, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel)
        {
            return await _transactionRepository.GetCountInCategoryAsync(categoryId, cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllInCategoryAsync(int walletId, int categoryId, CancellationToken cancel)
        {
            return GetItem(await _transactionRepository.GetAllInCategoryAsync(walletId, categoryId, cancel).ConfigureAwait(false));
        }

        public override async Task<TransactionDTO> AddAsync(TransactionCreateDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _transactionRepository.CheckEntitiesExistAsync(item.CategoryId, null, cancel).ConfigureAwait(false))
                throw new InvalidOperationException("Category not found.");

            return GetItem(await _transactionRepository.AddAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public override async Task<TransactionDTO> UpdateAsync(TransactionDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _transactionRepository.CheckEntitiesExistAsync(item.CategoryId, item.Id, cancel).ConfigureAwait(false))
                throw new InvalidOperationException("Category or transaction not found.");

            return GetItem(await _transactionRepository.UpdateAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public async Task<IPage<TransactionDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize,
            int? walletId = null, int? categoryId = null, CancellationToken cancel = default)
        {
            var result = await _transactionRepository
                .GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, categoryId, cancel).ConfigureAwait(false);

            return new Page(GetItem(result.Items), result.TotalCount, result.PageIndex, result.PageSize);
        }
    }
}
