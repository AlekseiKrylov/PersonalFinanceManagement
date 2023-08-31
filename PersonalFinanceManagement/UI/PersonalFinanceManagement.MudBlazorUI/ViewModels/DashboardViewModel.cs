using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.BLLModels;
using PersonalFinanceManagement.Domain.BLLModels.Reports;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels
{
    public class DashboardViewModel : ComponentBase
    {
        protected decimal _reportIncome;
        protected decimal _reportExpenses;
        protected List<TransactionWithCategory> _transactions = new();
        protected DailyTransactionsReport _currentDayTransactionsReport = new();
        protected PeriodTransactionsReport _currentMonthTransactionsReport = new();
        protected PeriodTransactionsReport _periodTransactionsReport = new();

        [Inject] IReportsWebApiClient ReportsWebApiClient { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    _dailyTransactionsReport = await ReportsWebApiClient.GetDailyReport(1, DateTime.Now.Date);
        //}

        protected async Task GetCurrentDayReportAsync(int walletId)
        {
            _currentDayTransactionsReport = await ReportsWebApiClient.GetDailyReport(walletId, DateTime.Now.Date);
            SetData(_currentDayTransactionsReport);
        }

        protected async Task GetCurrentMonthReportAsync(int walletId)
        {
            DateTime firstDayOfMonth = new(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            Console.WriteLine(firstDayOfMonth.ToString());
            Console.WriteLine(lastDayOfMonth.ToString());
            _currentMonthTransactionsReport = await ReportsWebApiClient.GetPeriodReport(walletId, firstDayOfMonth, lastDayOfMonth);
            SetData(_currentMonthTransactionsReport);
        }

        protected async Task GetPeriodReportAsync(int walletId, DateTime startDate, DateTime endDate)
        {
            _periodTransactionsReport = await ReportsWebApiClient.GetPeriodReport(walletId, startDate, endDate);
            SetData(_periodTransactionsReport);
        }

        protected void ClearData()
        {
            _reportIncome = default;
            _reportExpenses = default;
            _transactions = new();
        }

        private void SetData(TransactionsReport report)
        {
            _reportIncome = report.TotalIncome;
            _reportExpenses = report.TotalExpenses;
            _transactions = report.Transactions.ToList();
        }
    }
}
