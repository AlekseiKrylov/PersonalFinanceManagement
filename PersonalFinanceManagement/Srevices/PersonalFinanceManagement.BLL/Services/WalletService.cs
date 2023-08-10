using AutoMapper;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.ModelsDTO;

namespace PersonalFinanceManagement.BLL.Services
{
    public class WalletService : EntityServiceBase<WalletDTO, Wallet>
    {
        public WalletService(IWalletRepository walletRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(walletRepository, mapper)
        {
            if (int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("id")?.Value, out int userId) && userId > 0)
                walletRepository.SetUserId(userId);
            else
                throw new UnauthorizedAccessException("User is not authorized.");
        }
    }
}
