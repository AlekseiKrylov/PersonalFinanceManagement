using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        private readonly int _userId;

        protected override IQueryable<Wallet> Items => _userId > 0
            ? Set.Where(w => w.UserId == _userId)
            : Enumerable.Empty<Wallet>().AsQueryable();

        public WalletRepository(PFMDbContext db, ICurrentUserService currentUserService) : base(db)
            => _userId = currentUserService.GetCurretUserId();
    }
}
