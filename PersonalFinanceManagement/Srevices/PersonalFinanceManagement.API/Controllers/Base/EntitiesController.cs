using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.Interfaces.Entities;
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

        public EntitiesController(IEntityService<T, TCreate, TBase> entityService) => _entityService = entityService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> GetAll(CancellationToken cancel = default) => Ok(await _entityService.GetAllAsync(cancel));

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetItemsCount(CancellationToken cancel = default) => Ok(await _entityService.GetCountAsync(cancel));

        [HttpGet("exists/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ExistId(int id, CancellationToken cancel = default) =>
            await _entityService.ExistsByIdAsync(id, cancel) ? Ok(true) : NotFound(false);

        [HttpPost("exists")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Exist(T item, CancellationToken cancel = default) =>
            await _entityService.ExistsAsync(item, cancel) ? Ok(true) : NotFound(false);

        [HttpGet("items[[{skip:int}:{count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count, CancellationToken cancel = default) =>
            Ok(await _entityService.GetAsync(skip, count, cancel));

        [HttpGet("page[[{pageIndex:int}:{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<T>>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var result = await _entityService.GetPageAsync(pageIndex, pageSize, cancel);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> GetById(int id, CancellationToken cancel = default) =>
            await _entityService.GetByIdAsync(id, cancel) is { } item ? Ok(item) : NotFound();

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<T>> Add(TCreate item, CancellationToken cancel = default)
        {
            var result = await _entityService.AddAsync(item, cancel);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Update(T item, CancellationToken cancel = default)
        {
            var result = await _entityService.UpdateAsync(item, cancel);
            if (result == null)
                return NotFound(item);

            return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<T>> Delete(T item, CancellationToken cancel = default)
        {
            var result = await _entityService.RemoveAsync(item, cancel);
            if (result == null)
                return NotFound(item);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancel = default)
        {
            var result = await _entityService.RemoveByIdAsync(id, cancel);
            if (result == null)
                return NotFound(id);

            return Ok(result);
        }
    }
}
