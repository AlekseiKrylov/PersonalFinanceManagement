using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.DAL.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class CategoriesController : EntitiesController<Category>
    {
        public CategoriesController(IRepository<Category> repository) : base(repository) { }
    }
}
