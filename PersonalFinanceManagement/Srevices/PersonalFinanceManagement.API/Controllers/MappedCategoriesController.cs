using AutoMapper;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.ModelsDTO;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class MappedCategoriesController : MappedEntitiesController<CategoryDTO, Category>
    {
        public MappedCategoriesController(IRepository<Category> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
