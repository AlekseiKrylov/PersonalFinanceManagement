using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class WalletsRepository : RepositoryBase<Wallet>, IWalletsRepository
    {
        private readonly int _userId;

        protected override IQueryable<Wallet> Items => _userId > 0
            ? Set.Where(w => w.UserId == _userId)
            : Enumerable.Empty<Wallet>().AsQueryable();

        public WalletsRepository(PFMDbContext db, ICurrentUserService currentUserService) : base(db)
            => _userId = currentUserService.GetCurretUserId();
    }
}
