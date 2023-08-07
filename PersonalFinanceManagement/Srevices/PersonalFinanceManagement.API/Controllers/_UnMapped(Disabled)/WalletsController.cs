using PersonalFinanceManagement.API.Controllers.UnMapped.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers.UnMapped
{
    public class WalletsController : EntitiesController<Wallet>
    {
        public WalletsController(IRepository<Wallet> repository) : base(repository) { }
    }
}
