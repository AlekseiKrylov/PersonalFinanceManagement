using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.ModelsDTO;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    public class WalletsController : EntitiesController<WalletDTO, Wallet>
    {
        public WalletsController(IEntityService<WalletDTO, Wallet> walletService) : base(walletService) { }
    }
}
