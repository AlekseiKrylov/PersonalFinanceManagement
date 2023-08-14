using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.BLLModels.Reports
{
    public class PeriodTransactionsReport : TransactionsReport
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
