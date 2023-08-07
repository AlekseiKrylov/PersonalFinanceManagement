using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.BLLModels.Reports
{
    public class DailyTransactionsReport : TransactionsReport
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
