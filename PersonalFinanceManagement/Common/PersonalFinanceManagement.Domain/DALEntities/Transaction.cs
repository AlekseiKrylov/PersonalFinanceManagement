using PersonalFinanceManagement.Domain.DALEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Domain.DALEntities
{
    public class Transaction : Entity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }

        [Required, Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [Required]
        public int WalletId { get; set; }

        [ForeignKey(nameof(WalletId))]
        public virtual Wallet Wallet { get; set; }
    }
}
