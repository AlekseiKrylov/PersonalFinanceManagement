using PersonalFinanceManagement.API.Controllers.UnMapped.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers.UnMapped
{
    public class TransactionsController : EntitiesController<Transaction>
    {
        public TransactionsController(IRepository<Transaction> repository) : base(repository) { }
    }
}
