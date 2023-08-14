using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.API.Controllers
{
    public class TransactionsController : EntitiesController<TransactionDTO, TransactionCreateDTO, Transaction>
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService) : base(transactionService) => _transactionService = transactionService;

        [HttpPost("move")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> MoveTransactionsToAnotherCategory(int walletId, int sourceCategoryId, int targetCategoryId) =>
            await _transactionService.MoveToAnotherCategoryAsync(walletId, sourceCategoryId, targetCategoryId).ConfigureAwait(false)
            ? Ok(true)
            : NotFound(false);

        [HttpGet("count-in-category")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetCountInCategory(int walletId, int categoryId) =>
            Ok(await _transactionService.GetCountInCategoryAsync(walletId, categoryId).ConfigureAwait(false));

        [HttpGet("transactions-in-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllInCategory(int walletId, int categoryId) =>
            Ok(await _transactionService.GetAllInCategoryAsync(walletId, categoryId).ConfigureAwait(false));

        [HttpGet("page-with-restriction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<TransactionDTO>>> GetPage(int pageIndex, int pageSize, int? walletId = null, int? categoryId = null)
        {
            var result = await _transactionService.GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, categoryId);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }
    }
}
