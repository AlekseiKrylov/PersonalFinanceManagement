using AutoMapper;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.ModelsDTO;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class MappedWalletsController : MappedEntitiesController<WalletDTO, Wallet>
    {
        public MappedWalletsController(IRepository<Wallet> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
