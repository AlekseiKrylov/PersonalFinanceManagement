using PersonalFinanceManagement.Interfaces.Base.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.Interfaces.Services
{
    public interface IEntityService<T, TBase> where T : IEntity where TBase : IEntity
    {
        Task<bool> ExistsByIdAsync(int id, CancellationToken cancel = default);
        Task<bool> ExistsAsync(T item, CancellationToken cancel = default);
        Task<int> GetCountAsync(CancellationToken cancel = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default);
        Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default);
        Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancel = default);
        Task<T> AddAsync(T item, CancellationToken cancel = default);
        Task<T> UpdateAsync(T item, CancellationToken cancel = default);
        Task<T> RemoveByIdAsync(int id, CancellationToken cancel = default);
        Task<T> RemoveAsync(T item, CancellationToken cancel = default);
    }
}
