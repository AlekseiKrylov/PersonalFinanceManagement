using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        private int _userId;
        
        protected override IQueryable<Wallet> Items => _userId > 0 ? Set.Where(w => w.UserId == _userId) : Set;

        public WalletRepository(PFMDbContext db) : base(db) { }

        public void SetUserId(int userId) => _userId = userId;
    }
}
