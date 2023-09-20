using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.API.Controllers.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Common;

namespace PersonalFinanceManagement.API.Controllers
{
    public class CategoriesController : EntitiesController<CategoryDTO, CategoryCreateDTO, Category>
    {
        private readonly ICategoriesServise _categoriesService;

        public CategoriesController(ICategoriesServise categoriesService) : base(categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("in-wallet-count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetCountInWallet(int walletId, CancellationToken cancel = default) =>
            Ok(await _categoriesService.GetCountInWalletAsync(walletId, cancel).ConfigureAwait(false));

        [HttpGet("in-wallet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllInWallet(int walletId, CancellationToken cancel = default) =>
            Ok(await _categoriesService.GetAllInWalletAsync(walletId, cancel).ConfigureAwait(false));

        [HttpGet("page-with-restriction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<CategoryDTO>>> GetPage(int pageIndex,
                                                                       int pageSize,
                                                                       int? walletId = null,
                                                                       CancellationToken cancel = default)
        {
            var result = await _categoriesService.GetPageWithRestrictionsAsync(pageIndex, pageSize, walletId, cancel);
            return result.Items.Any() ? Ok(result) : NotFound(result);
        }
    }
}
