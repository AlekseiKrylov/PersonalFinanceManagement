using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.Interfaces.Entities;

namespace PersonalFinanceManagement.Interfaces.WebApiClients
{
    public interface IEntitiesWebApiClient<T, TCreate>
        where T : IEntity
        where TCreate : IEntity
    {
        Task<bool> ExistIdAsync(int id, CancellationToken cancel = default);
        Task<bool> ExistAsync(T item, CancellationToken cancel = default);
        Task<int> GetCountAsync(CancellationToken cancel = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default);
        Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default);
        Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancel = default);
        Task<T> AddAsync(TCreate item, CancellationToken cancel = default);
        Task<T> UpdateAsync(T item, CancellationToken cancel = default);
        Task<T> DeleteByIdAsync(int id, CancellationToken cancel = default);
        Task<T> DeleteAsync(T item, CancellationToken cancel = default);
    }
}