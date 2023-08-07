using AutoMapper;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.ModelsDTO;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class MappedTransactionsController : MappedEntitiesController<TransactionDTO, Transaction>
    {
        public MappedTransactionsController(IRepository<Transaction> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
