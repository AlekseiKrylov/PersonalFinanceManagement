using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Entities.Base;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly PFMDbContext _db;

        protected DbSet<T> Set { get; }

        protected virtual IQueryable<T> Items => Set;

        public DbRepository(PFMDbContext db)
        {
            _db = db;
            Set = _db.Set<T>();
        }

        public async Task<bool> ExistByIdAsync(int id, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(i => i.Id == id, cancel).ConfigureAwait(false);
        }

        public async Task<bool> ExistAsync(T item, CancellationToken cancel = default)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return await Items.AnyAsync(i => i.Id == item.Id, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync(CancellationToken cancel = default)
        {
            return await Items.CountAsync(cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default)
        {
            return await Items.ToArrayAsync(cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default)
        {
            //return await Items.Skip(skip).Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
            if (count <= 0)
                return Enumerable.Empty<T>();

            var query = Items;
            if (skip > 0)
                query = query.Skip(skip);

            return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
        }

        protected record Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPage<T>;

        public async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            if (pageSize <= 0)
                return new Page(Enumerable.Empty<T>(), await GetCountAsync(cancel).ConfigureAwait(false), pageIndex, pageSize);
            
            var query = Items;
            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0)
                return new Page(Enumerable.Empty<T>(), totalCount, pageIndex, pageSize);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);
            
            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page(items, totalCount, pageIndex, pageSize);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancel = default)
        {
            return await Items.FirstOrDefaultAsync(i => i.Id == id, cancel).ConfigureAwait(false);

            //return Items switch
            //{
            //    DbSet<T> set => await set.FindAsync(new object[] { id }, cancel).ConfigureAwait(false),
            //    IQueryable<T> items => await Items.FirstOrDefaultAsync(i => i.Id == id, cancel).ConfigureAwait(false),
            //    _ => throw new InvalidOperationException("Data source identification error"),
            //};
        }

        public async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            //_db.Entry(item).State = EntityState.Added;
            //await Set.AddAsync(item, cancel);
            await _db.AddAsync(item, cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            //_db.Entry(item).State = EntityState.Modified;
            //Set.Update(item);
            _db.Update(item);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<T> RemoveByIdAsync(int id, CancellationToken cancel = default)
        {
            var item = await GetByIdAsync(id, cancel).ConfigureAwait(false);
            if (item is null)
                return null;

            return await RemoveAsync(item, cancel).ConfigureAwait(false);
        }

        public async Task<T> RemoveAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await ExistByIdAsync(item.Id, cancel))
                return null;

            //_db.Entry(item).State = EntityState.Deleted;
            //Set.Remove(item);
            _db.Remove(item);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }
    }
}
