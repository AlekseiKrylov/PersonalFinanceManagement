using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Domain.BLLModels.Reports
{
    public class TransactionsReport
    {
        public int WalletId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalIncome { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalExpenses { get; set; }

        public List<TransactionWithCategory> Transactions { get; set; }
    }
}
