using AutoMapper;
using PersonalFinanceManagement.Interfaces.Base.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services.Base
{
    public class EntityServiceBase<T, TCreate, TBase> : IEntityService<T, TCreate, TBase>
        where T : IEntity
        where TCreate : IEntity
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
        protected virtual TBase GetBase(TCreate item) => _mapper.Map<TBase>(item);
        protected virtual T GetItem(TBase item) => _mapper.Map<T>(item);
        protected virtual IEnumerable<TBase> GetBase(IEnumerable<T> items) => _mapper.Map<IEnumerable<TBase>>(items);
        protected virtual IEnumerable<T> GetItem(IEnumerable<TBase> items) => _mapper.Map<IEnumerable<T>>(items);
        protected IPage<T> GetItem(IPage<TBase> tPage) => new Page(GetItem(tPage.Items), tPage.TotalCount, tPage.PageIndex, tPage.PageSize);
        protected record Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPage<T>
        {
            public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        };

        public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancel)
        {
            if (id <= 0)
                throw new ArgumentException($"Invalid {nameof(id)} value. ID must be greater than 0.");

            return await _repository.ExistsByIdAsync(id, cancel).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(T item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return await _repository.ExistsAsync(GetBase(item), cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync(CancellationToken cancel)
        {
            return await _repository.GetCountAsync(cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel)
        {
            return GetItem(await _repository.GetAllAsync(cancel).ConfigureAwait(false));
        }

        public async Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel)
        {
            return GetItem(await _repository.GetAsync(skip, count, cancel).ConfigureAwait(false));
        }

        public async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel)
        {
            var result = await _repository.GetPageAsync(pageIndex, pageSize, cancel).ConfigureAwait(false);

            return new Page(GetItem(result.Items), result.TotalCount, result.PageIndex, result.PageSize);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancel)
        {
            if (id <= 0)
                throw new ArgumentException($"Invalid {nameof(id)} value. ID must be greater than 0.");

            return GetItem(await _repository.GetByIdAsync(id, cancel).ConfigureAwait(false));
        }

        public virtual async Task<T> AddAsync(TCreate item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return GetItem(await _repository.AddAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public virtual async Task<T> UpdateAsync(T item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return GetItem(await _repository.UpdateAsync(GetBase(item), cancel).ConfigureAwait(false));
        }

        public async Task<T> RemoveByIdAsync(int id, CancellationToken cancel)
        {
            if (id <= 0)
                throw new ArgumentException($"Invalid {nameof(id)} value. ID must be greater than 0.");

            return GetItem(await _repository.RemoveByIdAsync(id, cancel).ConfigureAwait(false));
        }

        public async Task<T> RemoveAsync(T item, CancellationToken cancel)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return GetItem(await _repository.RemoveAsync(GetBase(item), cancel).ConfigureAwait(false));
        }
    }
}
