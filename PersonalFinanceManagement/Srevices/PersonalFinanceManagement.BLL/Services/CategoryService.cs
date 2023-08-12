using AutoMapper;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.BLL.Services
{
    public class CategoryService : EntityServiceBase<CategoryDTO, CategoryCreateDTO, Category>, ICategoryServise
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(categoryRepository, mapper)
        {
            if (!int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("id")?.Value, out int userId) && userId <= 0)
                throw new UnauthorizedAccessException("User is not authorized.");

            categoryRepository.SetUserId(userId);

            _categoryRepository = categoryRepository;
        }

        public override async Task<CategoryDTO> AddAsync(CategoryCreateDTO item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _categoryRepository.CheckEntitiesExistAsync(item.WalletId, null, cancel))
                throw new InvalidOperationException("Wallet not found.");

            return GetItem(await _categoryRepository.AddAsync(GetBase(item), cancel));
        }

        public override async Task<CategoryDTO> UpdateAsync(CategoryDTO item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _categoryRepository.CheckEntitiesExistAsync(item.WalletId, item.Id, cancel))
                throw new InvalidOperationException("Wallet or category not found.");

            return GetItem(await _categoryRepository.UpdateAsync(GetBase(item), cancel));
        }

        public async Task<IPage<CategoryDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize,
            int? walletId = null, CancellationToken cancel = default)
        {
            var result = await _categoryRepository
                .GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, cancel)
                .ConfigureAwait(false);

            return new Page(GetItem(result.Items), result.TotalCount, result.PageIndex, result.PageSize);
        }
    }
}
