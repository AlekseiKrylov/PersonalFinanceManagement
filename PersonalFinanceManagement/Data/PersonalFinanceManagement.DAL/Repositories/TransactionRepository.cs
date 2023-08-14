using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        private readonly PFMDbContext _db;
        private int _userId;

        protected override IQueryable<Transaction> Items => _userId > 0 ? Set.Where(t => t.Wallet.UserId == _userId) : Set;

        public TransactionRepository(PFMDbContext db) : base(db) => _db = db;

        public void SetUserId(int userId) => _userId = userId;

        public async Task<IEnumerable<Transaction>> GetPeriodWithCategoryAsync(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default)
        {
            if (endDate < startDate)
                return Enumerable.Empty<Transaction>();

            var query = Items is IOrderedQueryable<Transaction> ? Items : Items.OrderBy(i => i.Id);

            return await query
                .Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date && t.WalletId == walletId)
                .Include(t => t.Category)
                .ToArrayAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> MoveToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default)
        {
            var uniqueCategoryCount = await Items
                .Where(t => t.WalletId == walletId && (t.CategoryId == sourceCategoryId || t.CategoryId == targetCategoryId))
                .Select(t => t.CategoryId)
                .Distinct()
                .CountAsync(cancel)
                .ConfigureAwait(false);

            if (uniqueCategoryCount != 2)
                return false;

            var totalTransactionsToUpdate = await GetCountInCategoryAsync(walletId, targetCategoryId, cancel);

            if (totalTransactionsToUpdate == 0)
                return false;

            int batchSize = 1000;

            while (totalTransactionsToUpdate > 0)
            {
                var transactionsToUpdate = await _db.Transactions
                    .Where(t => t.WalletId == walletId && t.CategoryId == sourceCategoryId)
                    .Take(batchSize).ToListAsync(cancel).ConfigureAwait(false);

                foreach (var transaction in transactionsToUpdate)
                    transaction.CategoryId = targetCategoryId;

                await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
                totalTransactionsToUpdate -= transactionsToUpdate.Count;
            }

            return true;
        }

        public async Task<int> GetCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default)
        {
            return await Items.Where(t => t.WalletId == walletId && t.CategoryId == categoryId).CountAsync(cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Transaction>> GetAllInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default)
        {
            var query = Items is IOrderedQueryable<Transaction> ? Items : Items.OrderBy(i => i.Id);
            return await query
                .Where(t => t.WalletId == walletId && t.CategoryId == categoryId)
                .ToArrayAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> CheckEntitiesExistAsync(int walletId, int? categoryId, int? transactionId, CancellationToken cancel = default)
        {
            var query = _db.Wallets.Where(w => w.UserId == _userId && w.Id == walletId);

            if (categoryId.HasValue)
                query = query.Join(_db.Categories.Where(c => c.WalletId == walletId && c.Id == categoryId), w => w.Id, c => c.WalletId, (w, c) => w);

            if (transactionId.HasValue)
                query = query.Join(_db.Transactions.Where(t => t.WalletId == walletId && t.Id == transactionId), w => w.Id, t => t.WalletId, (w, t) => w);

            var result = await query.AnyAsync(cancel).ConfigureAwait(false);
            return result;
        }

        public async Task<IPage<Transaction>> GetPageWithRestrictionsAsync(int pageIndex, int pageSize,
            int? walletId = null, int? categoryId = null, CancellationToken cancel = default)
        {
            if (!walletId.HasValue && !categoryId.HasValue)
                return await base.GetPageAsync(pageIndex, pageSize, cancel).ConfigureAwait(false);

            if (pageSize <= 0)
                return new Page(Enumerable.Empty<Transaction>(), await GetCountAsync(cancel).ConfigureAwait(false), pageIndex, pageSize);

            var query = Items;
            if (walletId is not null)
                query = query.Where(t => t.WalletId == walletId);

            if (categoryId is not null)
                query = query.Where(t => t.CategoryId == categoryId);

            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0 || pageIndex >= totalCount)
                return new Page(Enumerable.Empty<Transaction>(), totalCount, pageIndex, pageSize);

            if (query is not IOrderedQueryable<Transaction>)
                query = query.OrderBy(i => i.Id);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);

            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page(items, totalCount, pageIndex, pageSize);
        }
    }
}
