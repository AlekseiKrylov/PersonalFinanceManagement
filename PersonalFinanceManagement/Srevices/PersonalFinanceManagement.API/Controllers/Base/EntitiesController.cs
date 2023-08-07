using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Interfaces.Base.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers.Base
{
    [ApiController, Route("api/[controller]")]
    public abstract class EntitiesController<T, TBase> : ControllerBase
        where T : IEntity
        where TBase : IEntity
    {
        private readonly IRepository<TBase> _repository;
        private readonly IMapper _mapper;

        public EntitiesController(IRepository<TBase> repository, IMapper mapper)
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


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> GetAll() => Ok(GetItem(await _repository.GetAllAsync()));

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetItemsCount() => Ok(await _repository.GetCountAsync());

        [HttpGet("exist/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistId(int id) => await _repository.ExistByIdAsync(id) ? Ok(true) : NotFound(false);

        [HttpPost("exist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(T item) => await _repository.ExistAsync(GetBase(item)) ? Ok(true) : NotFound(false);

        [HttpGet("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count) => Ok(GetItem(await _repository.GetAsync(skip, count)));


        [HttpGet("page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<T>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _repository.GetPageAsync(pageIndex, pageSize);
            return result.Items.Any() ? Ok(GetItem(result)) : NotFound(result);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> GetById(int id) => await _repository.GetByIdAsync(id) is { } item ? Ok(GetItem(item)) : NotFound();

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<T>> Add(T item)
        {
            var result = await _repository.AddAsync(GetBase(item));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, GetItem(result));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Update(T item)
        {
            if (await _repository.UpdateAsync(GetBase(item)) is not { } result)
                return NotFound(item);

            return AcceptedAtAction(nameof(Get), new { id = result.Id }, GetItem(result));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Delete(T item)
        {
            if (await _repository.RemoveAsync(GetBase(item)) is not { } result)
                return NotFound(item);

            return Ok(GetItem(result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _repository.RemoveByIdAsync(id) is not { } result)
                return NotFound(id);

            return Ok(GetItem(result));
        }
    }
}
