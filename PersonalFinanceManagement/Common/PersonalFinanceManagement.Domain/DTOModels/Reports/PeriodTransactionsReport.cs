using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DTOModels.Reports
{
    public class PeriodTransactionsReport : TransactionsReport
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
