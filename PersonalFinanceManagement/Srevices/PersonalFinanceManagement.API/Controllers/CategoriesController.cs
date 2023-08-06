using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class CategoriesController : EntitiesController<Category>
    {
        public CategoriesController(IRepository<Category> repository) : base(repository) { }
    }
}
