using AutoMapper;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repositories;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.BLL.Services
{
    public class TransactionsService : EntitiesServiceBase<TransactionDTO, TransactionCreateDTO, Transaction>, ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsService(ITransactionsRepository transactionsRepository, IMapper mapper) : base(transactionsRepository, mapper)
            => _transactionsRepository = transactionsRepository;

        public async Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel)
        {
            return await _transactionsRepository.MoveToAnotherCategoryOfSameWalletAsync(walletId, sourceCategoryId, targetCategoryId, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCountInCategoryAsync(int categoryId, CancellationToken cancel)
        {
            return await _transactionsRepository.GetCountInCategoryAsync(categoryId, cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllInCategoryAsync(int categoryId, CancellationToken cancel)
        {
            return GetItem(await _transactionsRepository.GetAllInCategoryAsync(categoryId, cancel).ConfigureAwait(false));
        }

        public override async Task<TransactionDTO> AddAsync(TransactionCreateDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _transactionsRepository.CheckEntitiesExistAsync(item.CategoryId, null, cancel).ConfigureAwait(false))
                throw new InvalidOperationException("Category not found.");

            return GetItem(await _transactionsRepository.AddAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public override async Task<TransactionDTO> UpdateAsync(TransactionDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _transactionsRepository.CheckEntitiesExistAsync(item.CategoryId, item.Id, cancel).ConfigureAwait(false))
                throw new InvalidOperationException("Category or transaction not found.");

            return GetItem(await _transactionsRepository.UpdateAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public async Task<IPage<TransactionDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize,
            int? walletId = null, int? categoryId = null, CancellationToken cancel = default)
        {
            var result = await _transactionsRepository
                .GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, categoryId, cancel).ConfigureAwait(false);

            return new Page(GetItem(result.Items), result.TotalCount, result.PageIndex, result.PageSize);
        }
    }
}
