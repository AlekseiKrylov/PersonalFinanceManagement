using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.Domain.Interfaces.Services
{
    public interface ICategoriesServise : IEntityService<CategoryDTO, CategoryCreateDTO, Category>
    {
        Task<int> GetCountInWalletAsync(int walletId, CancellationToken cancel = default);
        Task<IEnumerable<CategoryDTO>> GetAllInWalletAsync(int walletId, CancellationToken cancel = default);
        Task<IPage<CategoryDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default);
    }
}
