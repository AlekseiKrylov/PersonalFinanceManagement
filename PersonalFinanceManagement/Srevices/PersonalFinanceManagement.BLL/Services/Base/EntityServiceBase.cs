using AutoMapper;
using PersonalFinanceManagement.Interfaces.Base.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services.Base
{
    public class EntityServiceBase<T, TBase> : IEntityService<T, TBase>
    where T : IEntity
    where TBase : IEntity
    {
        private readonly IRepository<TBase> _repository;
        private readonly IMapper _mapper;

        public EntityServiceBase(IRepository<TBase> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected virtual TBase GetBase(T item) => _mapper.Map<TBase>(item);
        protected virtual T GetItem(TBase item) => _mapper.Map<T>(item);
        protected virtual IEnumerable<TBase> GetBase(IEnumerable<T> items) => _mapper.Map<IEnumerable<TBase>>(items);
        protected virtual IEnumerable<T> GetItem(IEnumerable<TBase> items) => _mapper.Map<IEnumerable<T>>(items);
        protected IPage<T> GetItem(IPage<TBase> tPage) => new Page(GetItem(tPage.Items), tPage.TotalCount, tPage.PageIndex, tPage.PageSize);
        protected record Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPage<T>
        {
            public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        };

        public Task<bool> ExistsByIdAsync(int id, CancellationToken cancel = default)
        {
            return _repository.ExistsByIdAsync(id, cancel);
        }

        public Task<bool> ExistsAsync(T item, CancellationToken cancel = default)
        {
            return _repository.ExistsAsync(GetBase(item), cancel);
        }

        public Task<int> GetCountAsync(CancellationToken cancel = default)
        {
            return _repository.GetCountAsync(cancel);
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default)
        {
            return Task.FromResult(GetItem(_repository.GetAllAsync(cancel).Result));
        }

        public Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default)
        {
            return Task.FromResult(GetItem(_repository.GetAsync(skip, count, cancel).Result));
        }

        public async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var result = await _repository.GetPageAsync(pageIndex, pageSize, cancel);
            return new Page(GetItem(result.Items), result.TotalCount, result.PageIndex, result.PageSize);
        }

        public Task<T> GetByIdAsync(int id, CancellationToken cancel = default)
        {
            return Task.FromResult(GetItem(_repository.GetByIdAsync(id, cancel).Result));
        }

        public async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            var mappedItem = GetBase(item);
            var result = await _repository.AddAsync(mappedItem, cancel);
            return GetItem(result);
        }

        public async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            var mappedItem = GetBase(item);
            var result = await _repository.UpdateAsync(mappedItem, cancel);
            return GetItem(result);
        }

        public async Task<T> RemoveByIdAsync(int id, CancellationToken cancel = default)
        {
            var result = await _repository.RemoveByIdAsync(id, cancel);
            return GetItem(result);
        }

        public async Task<T> RemoveAsync(T item, CancellationToken cancel = default)
        {
            var mappedItem = GetBase(item);
            var result = await _repository.RemoveAsync(mappedItem, cancel);
            return GetItem(result);
        }
    }
}
