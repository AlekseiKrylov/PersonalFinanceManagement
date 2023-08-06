using PersonalFinanceManagement.Domain.DALEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Domain.DALEntities
{
    public class Category : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsIncome { get; set; }

        [Required]
        public int WalletId { get; set; }

        [ForeignKey(nameof(WalletId))]
        public virtual Wallet Wallet { get; set; }

        //public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
