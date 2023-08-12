using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Base.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.Domain.Interfaces.Services
{
    public interface ICategoryServise : IEntityService<CategoryDTO, CategoryCreateDTO, Category>
    {
        Task<IPage<CategoryDTO>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default);
    }
}
