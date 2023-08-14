using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

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
