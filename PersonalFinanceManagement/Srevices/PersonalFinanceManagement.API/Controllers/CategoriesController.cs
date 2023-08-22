using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    public class CategoriesController : EntitiesController<CategoryDTO, CategoryCreateDTO, Category>
    {
        public CategoriesController(ICategoriesServise categoryService) : base(categoryService) { }
    }
}
