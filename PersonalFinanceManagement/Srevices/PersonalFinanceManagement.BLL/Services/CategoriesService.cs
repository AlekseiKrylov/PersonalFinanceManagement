using AutoMapper;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repositories;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Common;

namespace PersonalFinanceManagement.BLL.Services
{
    public class CategoriesService : EntitiesServiceBase<CategoryDTO, CategoryCreateDTO, Category>, ICategoriesServise
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoryRepository, IMapper mapper) : base(categoryRepository, mapper)
            => _categoriesRepository = categoryRepository;

        public async Task<int> GetCountInWalletAsync(int walletId, CancellationToken cancel)
        {
            return await _categoriesRepository.GetCountInWalletAsync(walletId, cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllInWalletAsync(int walletId, CancellationToken cancel)
        {
            return GetItem(await _categoriesRepository.GetAllInWalletAsync(walletId, cancel).ConfigureAwait(false));
        }

        public override async Task<CategoryDTO> AddAsync(CategoryCreateDTO item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _categoriesRepository.CheckEntitiesExistAsync(item.WalletId, null, cancel))
                throw new InvalidOperationException("Wallet not found.");

            return GetItem(await _categoriesRepository.AddAsync(GetBase(item), cancel));
        }

        public override async Task<CategoryDTO> UpdateAsync(CategoryDTO item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _categoriesRepository.CheckEntitiesExistAsync(item.WalletId, item.Id, cancel))
                throw new InvalidOperationException("Wallet or category not found.");

            return GetItem(await _categoriesRepository.UpdateAsync(GetBase(item), cancel));
        }

        public async Task<IPage<CategoryDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize,
            int? walletId = null, CancellationToken cancel = default)
        {
            var result = await _categoriesRepository
                .GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, cancel)
                .ConfigureAwait(false);

            return new Page(GetItem(result.Items), result.TotalItems, result.PageIndex, result.PageSize);
        }
    }
}
