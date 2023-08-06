using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class TransactionsController : EntitiesController<Transaction>
    {
        public TransactionsController(IRepository<Transaction> repository) : base(repository) { }
    }
}
