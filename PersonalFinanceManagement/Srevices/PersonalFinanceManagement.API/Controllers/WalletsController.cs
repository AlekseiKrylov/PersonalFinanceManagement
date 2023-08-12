using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    public class WalletsController : EntitiesController<WalletDTO, WalletCreateDTO, Wallet>
    {
        public WalletsController(IEntityService<WalletDTO, WalletCreateDTO, Wallet> walletService) : base(walletService) { }
    }
}
