using AutoMapper;
using PersonalFinanceManagement.BLL.Services.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services
{
    public class WalletService : EntityServiceBase<WalletDTO, WalletCreateDTO, Wallet>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICurrentUserService _currentUserService;

        public WalletService(IWalletRepository walletRepository, IMapper mapper, ICurrentUserService currentUserService) : base(walletRepository, mapper)
        {
            _walletRepository = walletRepository;
            _currentUserService = currentUserService;
        }

        public override async Task<WalletDTO> AddAsync(WalletCreateDTO item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            int currentUserId = _currentUserService.GetCurretUserId();
            if (item.UserId != currentUserId)
                throw new ArgumentException($"Error! Wallet owner ID '{item.UserId}' must equal current user ID '{currentUserId}'.");

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
