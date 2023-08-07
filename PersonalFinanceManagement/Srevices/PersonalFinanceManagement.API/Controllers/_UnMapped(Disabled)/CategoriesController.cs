using PersonalFinanceManagement.API.Controllers.UnMapped.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers.UnMapped
{
    public class CategoriesController : EntitiesController<Category>
    {
        public CategoriesController(IRepository<Category> repository) : base(repository) { }
    }
}
