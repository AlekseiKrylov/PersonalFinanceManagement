using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.Domain.DALEntities.Base;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.DAL.Repositories.Base
{
    public class RepositoryBase<T> : IRepository<T> where T : Entity, new()
    {
        private readonly PFMDbContext _db;

        protected DbSet<T> Set { get; }

        protected virtual IQueryable<T> Items => Set;

        public RepositoryBase(PFMDbContext db)
        {
            _db = db;
            Set = _db.Set<T>();
        }

        public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(i => i.Id == id, cancel).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(T item, CancellationToken cancel = default)
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
            if (count <= 0)
                return Enumerable.Empty<T>();

            var query = Items is IOrderedQueryable<T> ? Items : Items.OrderBy(i => i.Id);

            if (skip > 0)
                query = query.Skip(skip);

            return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
        }

        protected record Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPage<T>
        {
            public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        };

        public virtual async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            if (pageSize <= 0)
                return new Page(Enumerable.Empty<T>(), await GetCountAsync(cancel).ConfigureAwait(false), pageIndex, pageSize);

            var query = Items;
            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0)
                return new Page(Enumerable.Empty<T>(), totalCount, pageIndex, pageSize);

            if (query is not IOrderedQueryable<T>)
                query = query.OrderBy(i => i.Id);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);

            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page(items, totalCount, pageIndex, pageSize);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancel = default)
        {
            return await Items.FirstOrDefaultAsync(i => i.Id == id, cancel).ConfigureAwait(false);
        }

        public async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            await _db.AddAsync(item, cancel).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

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

            if (!await ExistsByIdAsync(item.Id, cancel))
                return null;

            _db.Remove(item);
            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
            return item;
        }
    }
}
