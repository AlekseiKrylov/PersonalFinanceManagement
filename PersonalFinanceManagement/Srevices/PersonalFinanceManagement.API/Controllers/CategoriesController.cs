using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    public class CategoriesController : EntitiesController<CategoryDTO, CategoryCreateDTO, Category>
    {
        public CategoriesController(IEntityService<CategoryDTO, CategoryCreateDTO, Category> categoryService) : base(categoryService) { }
    }
}
