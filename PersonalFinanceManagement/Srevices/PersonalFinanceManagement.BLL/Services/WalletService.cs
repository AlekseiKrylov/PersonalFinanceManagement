using AutoMapper;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repository;

namespace PersonalFinanceManagement.BLL.Services
{
    public class WalletService : EntityServiceBase<WalletDTO, WalletCreateDTO, Wallet>
    {
        private readonly int _userId;
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(walletRepository, mapper)
        {
            if (!int.TryParse(httpContextAccessor.HttpContext.User.FindFirst("id")?.Value, out int userId) && userId <= 0)
                throw new UnauthorizedAccessException("User is not authorized.");

            walletRepository.SetUserId(userId);

            _userId = userId;
            _walletRepository = walletRepository;
        }

        public override async Task<WalletDTO> AddAsync(WalletCreateDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (item.UserId != _userId)
                throw new ArgumentException($"Error! Wallet owner ID '{item.UserId}' must equal current user ID '{_userId}'.");

            return GetItem(await _walletRepository.AddAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public override async Task<WalletDTO> UpdateAsync(WalletDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await _walletRepository.ExistsByIdAsync(item.Id, cancel).ConfigureAwait(false))
                throw new InvalidOperationException("Wallet not found.");

            return GetItem(await _walletRepository.UpdateAsync(GetBase(item), cancel).ConfigureAwait(false));
        }
    }
}
