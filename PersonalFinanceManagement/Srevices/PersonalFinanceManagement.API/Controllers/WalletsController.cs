using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.DAL.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class WalletsController : EntitiesController<Wallet>
    {
        public WalletsController(IRepository<Wallet> repository) : base(repository) { }
    }
}
