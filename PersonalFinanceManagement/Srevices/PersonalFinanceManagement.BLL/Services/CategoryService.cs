using AutoMapper;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.ModelsDTO;

namespace PersonalFinanceManagement.BLL.Services
{
    public class CategoryService : EntityServiceBase<CategoryDTO, Category>
    {
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(categoryRepository, mapper)
        {
            if (int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("id")?.Value, out int userId) && userId > 0)
                categoryRepository.SetUserId(userId);
            else
                throw new UnauthorizedAccessException("User is not authorized.");
        }
    }
}
