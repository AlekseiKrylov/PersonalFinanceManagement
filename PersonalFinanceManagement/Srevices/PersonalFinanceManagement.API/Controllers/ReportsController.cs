using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.Interfaces.Services;

namespace PersonalFinanceManagement.API.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService) => _reportsService = reportsService;

        [HttpGet("daily/wallet/{walletId:int}/{date}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DailyTransactionsReport>> GetDailyReport(int walletId, DateTime date, CancellationToken cancel) =>
            Ok(await _reportsService.GetDailyReport(walletId, date, cancel));

        [HttpGet("period/wallet/{walletId:int}/{startDate}/{endDate}/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PeriodTransactionsReport>> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel) =>
            Ok(await _reportsService.GetPeriodReport(walletId, startDate.Date, endDate.Date, cancel));
    }
}
