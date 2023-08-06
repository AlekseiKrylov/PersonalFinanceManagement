﻿using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Domain.Interfaces;

namespace PersonalFinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportController(IReportsService reportsService) => _reportsService = reportsService;

        [HttpGet("daily")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailyReport(int walletId, DateTime date, CancellationToken cancel) =>
            Ok(await _reportsService.GetDailyReport(walletId, date, cancel));

        [HttpGet("period")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPeriodReport(int walletId, DateTime startDate, DateTime endDate, CancellationToken cancel) =>
            Ok(await _reportsService.GetPeriodReport(walletId, startDate, endDate, cancel));
    }
}
