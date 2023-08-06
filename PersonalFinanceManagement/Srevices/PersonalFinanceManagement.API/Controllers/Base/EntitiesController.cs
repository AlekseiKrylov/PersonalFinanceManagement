using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.DALEntities.Base;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntitiesController<T> : ControllerBase where T : Entity
    {
        private readonly IRepository<T> _repository;

        protected EntitiesController(IRepository<T> repository) => _repository = repository;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

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
        public async Task<IActionResult> Exist(T item) => await _repository.ExistAsync(item) ? Ok(true) : NotFound(false);

        [HttpGet("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count) => Ok(await _repository.GetAsync(skip, count));

        [HttpGet("page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<T>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _repository.GetPageAsync(pageIndex, pageSize);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> GetById(int id) => await _repository.GetByIdAsync(id) is { } item ? Ok(item) : NotFound();

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(T item)
        {
            var result = await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(T item)
        {
            if (await _repository.UpdateAsync(item) is not { } result)
                return NotFound(item);

            return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(T item)
        {
            if (await _repository.RemoveAsync(item) is not { } result)
                return NotFound(item);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _repository.RemoveByIdAsync(id) is not { } result)
                return NotFound(id);

            return Ok(result);
        }
    }
}
