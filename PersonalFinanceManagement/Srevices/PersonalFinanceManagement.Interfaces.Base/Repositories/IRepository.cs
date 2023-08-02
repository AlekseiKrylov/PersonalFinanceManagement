using PersonalFinanceManagement.Interfaces.Base.Entities;

namespace PersonalFinanceManagement.Interfaces.Base.Repositories
{
    public interface IRepository<T> where T : IEntity, new()
    {
        Task<bool> ExistByIdAsync(int id, CancellationToken cancel = default);
        
        Task<bool> ExistAsync(T item, CancellationToken cancel = default);

        Task<int> GetCountAsync(CancellationToken cancel = default);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default);

        Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default);

        Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default);

        //async Task<T> GetByIdAsync(int id, CancellationToken cancel = default) => (await GetAllAsync(cancel)).FirstOrDefault(i => i.Id == id);
        Task<T> GetByIdAsync(int id, CancellationToken cancel = default);

        Task<T> AddAsync(T item, CancellationToken cancel = default);

        Task<T> UpdateAsync(T item, CancellationToken cancel = default);

        Task<T> RemoveByIdAsync(int id, CancellationToken cancel = default);
        
        Task<T> RemoveAsync(T item, CancellationToken cancel = default);
    }
}
