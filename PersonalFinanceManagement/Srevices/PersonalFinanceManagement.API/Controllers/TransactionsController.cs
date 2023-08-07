using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces;
using PersonalFinanceManagement.Domain.ModelsDTO;

namespace PersonalFinanceManagement.API.Controllers
{
    public class TransactionsController : EntitiesController<TransactionDTO, Transaction>
    {
        private readonly ITransactionRepository _repository;

        public TransactionsController(ITransactionRepository repository, IMapper mapper) : base(repository, mapper) => _repository = repository;

        [HttpPost("move")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> MoveTransactionsToAnotherCategory(int walletId, int sourceCategoryId, int targetCategoryId) =>
            await _repository.MoveToAnotherCategoryAsync(walletId, sourceCategoryId, targetCategoryId) ? Ok(true) : NotFound(false);

        [HttpGet("count-in-category")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetCountInCategory(int walletId, int categoryId) =>
            Ok(await _repository.GetCountInCategoryAsync(walletId, categoryId));

        [HttpGet("transactions-in-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllInCategory(int walletId, int categoryId) =>
            Ok(GetItem(await _repository.GetAllInCategoryAsync(walletId, categoryId)));
    }
}
