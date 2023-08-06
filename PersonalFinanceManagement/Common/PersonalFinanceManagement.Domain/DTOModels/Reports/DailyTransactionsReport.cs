using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DTOModels.Reports
{
    public class DailyTransactionsReport : TransactionsReport
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
