using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.ModelsDTO;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    public class CategoriesController : EntitiesController<CategoryDTO, Category>
    {
        public CategoriesController(IEntityService<CategoryDTO, Category> categoryService) : base(categoryService) { }
    }
}
