using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Interfaces.Base.Entities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers.Base
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public abstract class EntitiesController<T, TCreate, TBase> : ControllerBase
        where T : IEntity
        where TCreate : IEntity
        where TBase : IEntity
    {
        private readonly IEntityService<T, TCreate, TBase> _entityService;

        public EntitiesController(IEntityService<T, TCreate, TBase> entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> GetAll() => Ok(await _entityService.GetAllAsync());

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetItemsCount() => Ok(await _entityService.GetCountAsync());

        [HttpGet("exists/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistId(int id) => await _entityService.ExistsByIdAsync(id) ? Ok(true) : NotFound(false);

        [HttpPost("exists")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(T item) => await _entityService.ExistsAsync(item) ? Ok(true) : NotFound(false);

        [HttpGet("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count) => Ok(await _entityService.GetAsync(skip, count));

        [HttpGet("page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<T>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _entityService.GetPageAsync(pageIndex, pageSize);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> GetById(int id) => await _entityService.GetByIdAsync(id) is { } item ? Ok(item) : NotFound();

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<T>> Add(TCreate item)
        {
            var result = await _entityService.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Update(T item)
        {
            var result = await _entityService.UpdateAsync(item);
            if (result == null)
                return NotFound(item);

            return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Delete(T item)
        {
            var result = await _entityService.RemoveAsync(item);
            if (result == null)
                return NotFound(item);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _entityService.RemoveByIdAsync(id);
            if (result == null)
                return NotFound(id);

            return Ok(result);
        }
    }
}
