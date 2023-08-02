using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.DAL.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class TransactionsController : EntitiesController<Transaction>
    {
        public TransactionsController(IRepository<Transaction> repository) : base(repository) { }
    }
}
