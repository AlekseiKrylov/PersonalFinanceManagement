using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private int _userId;
        private readonly PFMDbContext _db;

        protected override IQueryable<Category> Items => _userId > 0 ? Set.Where(c => c.Wallet.UserId == _userId) : Set;

        public CategoryRepository(PFMDbContext db) : base(db) => _db = db;

        public void SetUserId(int userId) => _userId = userId;

        public async Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId, CancellationToken cancel = default)
        {
            var query = _db.Wallets.Where(w => w.UserId == _userId && w.Id == walletId);

            if (categoryId.HasValue)
                query = query.Join(_db.Categories.Where(c => c.WalletId == walletId && c.Id == categoryId), w => w.Id, c => c.WalletId, (w, c) => w);

            var result = await query.AnyAsync(cancel).ConfigureAwait(false);
            return result;
        }
    }
}
