using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repositories;
using PersonalFinanceManagement.Interfaces.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
    {
        private readonly PFMDbContext _db;
        private readonly int _userId;

        protected override IQueryable<Category> Items => _userId > 0
            ? Set.Where(c => c.Wallet.UserId == _userId)
            : Enumerable.Empty<Category>().AsQueryable();

        public CategoriesRepository(PFMDbContext db, ICurrentUserService currentUserService) : base(db)
        {
            _db = db;
            _userId = currentUserService.GetCurretUserId();
        }

        public async Task<int> GetCountInWalletAsync(int walletId, CancellationToken cancel = default)
        {
            return await Items.Where(t => t.WalletId == walletId).CountAsync(cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Category>> GetAllInWalletAsync(int walletId, CancellationToken cancel = default)
        {
            var query = Items is IOrderedQueryable<Transaction> ? Items : Items.OrderBy(i => i.Id);
            return await query.Where(t => t.WalletId == walletId).ToArrayAsync(cancel).ConfigureAwait(false);
        }

        public async Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId, CancellationToken cancel = default)
        {
            var query = _db.Wallets.Where(w => w.UserId == _userId && w.Id == walletId);

            if (categoryId.HasValue)
                query = query.Join(_db.Categories.Where(c => c.WalletId == walletId && c.Id == categoryId), w => w.Id, c => c.WalletId, (w, c) => w);

            var result = await query.AnyAsync(cancel).ConfigureAwait(false);
            return result;
        }

        public async Task<IPage<Category>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize, int? walletId = null, CancellationToken cancel = default)
        {
            if (!walletId.HasValue)
                return await base.GetPageAsync(pageIndex, pageSize, cancel).ConfigureAwait(false);

            if (pageSize <= 0)
                return new Page(Enumerable.Empty<Category>(), await GetCountAsync(cancel).ConfigureAwait(false), pageIndex, pageSize);

            var query = Items;
            if (walletId is not null)
                query = query.Where(t => t.WalletId == walletId);

            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0 || pageIndex >= totalCount)
                return new Page(Enumerable.Empty<Category>(), totalCount, pageIndex, pageSize);

            if (query is not IOrderedQueryable<Category>)
                query = query.OrderBy(i => i.Id);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);

            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page(items, totalCount, pageIndex, pageSize);
        }
    }
}
