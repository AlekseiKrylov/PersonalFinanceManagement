using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class DbTransactionRepository : DbRepositoryBase<Transaction>, ITransactionRepository
    {
        private readonly PFMDbContext _db;

        public DbTransactionRepository(PFMDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<Transaction>> GetTransactionsWithCategoryAsync(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel = default)
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

        public async Task<bool> MoveTransactionsToAnotherCategoryAsync(int walletId, int sourceCategoryId, int targetCategoryId, CancellationToken cancel = default)
        {
            var sourseCategoryIsExist = await _db.Categories.Where(c => c.WalletId == walletId && c.Id == sourceCategoryId).AnyAsync(cancel).ConfigureAwait(false);
            var targetCategoryIsExist = await _db.Categories.Where(c => c.WalletId == walletId && c.Id == targetCategoryId).AnyAsync(cancel).ConfigureAwait(false);
            if (!sourseCategoryIsExist || !targetCategoryIsExist)
                return false;

            int batchSize = 100;
            int totalTransactions = await GetTransactionCountInCategoryAsync(walletId, sourceCategoryId, cancel);
            while (totalTransactions > 0)
            {
                var transactionsToUpdate = await Items
                    .Where(c => c.WalletId == walletId && c.Id == sourceCategoryId).Take(batchSize).ToListAsync(cancel).ConfigureAwait(false);

                foreach (var transaction in transactionsToUpdate)
                    transaction.CategoryId = targetCategoryId;

                await _db.SaveChangesAsync(cancel).ConfigureAwait(false);

                totalTransactions -= transactionsToUpdate.Count;
            }

            return true;
        }

        public async Task<int> GetTransactionCountInCategoryAsync(int walletId, int categoryId, CancellationToken cancel = default)
        {
            return await Items.Where(t => t.WalletId == walletId && t.CategoryId == categoryId).CountAsync(cancel).ConfigureAwait(false);
        }
    }
}
