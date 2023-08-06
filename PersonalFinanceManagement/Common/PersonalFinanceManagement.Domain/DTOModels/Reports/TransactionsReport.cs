using PersonalFinanceManagement.Domain.DALEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Domain.DTOModels.Reports
{
    public class TransactionsReport
    {
        public int WalletId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalIncome { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalExpenses { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
