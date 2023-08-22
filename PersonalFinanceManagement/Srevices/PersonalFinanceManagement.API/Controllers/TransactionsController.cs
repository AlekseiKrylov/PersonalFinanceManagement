using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class TransactionsController : EntitiesController<TransactionDTO, TransactionCreateDTO, Transaction>
    {
        private readonly ITransactionsService _transactionService;

        public TransactionsController(ITransactionsService transactionService) : base(transactionService) => _transactionService = transactionService;

        [HttpPost("move[[{walletId:int}:{sourceCategoryId:int}:{targetCategoryId:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> MoveTransactionsToAnotherCategory(int walletId,
                                                                           int sourceCategoryId,
                                                                           int targetCategoryId,
                                                                           CancellationToken cancel = default) =>
            await _transactionService.MoveToAnotherCategoryAsync(walletId, sourceCategoryId, targetCategoryId, cancel).ConfigureAwait(false)
            ? Ok(true)
            : NotFound(false);

        [HttpGet("count-in-category[[{categoryId:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetCountInCategory(int categoryId, CancellationToken cancel = default) =>
            Ok(await _transactionService.GetCountInCategoryAsync(categoryId, cancel).ConfigureAwait(false));

        [HttpGet("transactions-in-category[[{categoryId:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllInCategory(int categoryId, CancellationToken cancel = default) =>
            Ok(await _transactionService.GetAllInCategoryAsync(categoryId, cancel).ConfigureAwait(false));

        [HttpGet("page-with-restriction[[{pageIndex}:{pageSize}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<TransactionDTO>>> GetPage(int pageIndex,
                                                                       int pageSize,
                                                                       int? walletId = null,
                                                                       int? categoryId = null,
                                                                       CancellationToken cancel = default)
        {
            var result = await _transactionService.GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, categoryId, cancel);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }
    }
}
