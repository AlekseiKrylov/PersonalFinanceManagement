using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.DAL.Entities
{
    [Index(nameof(Name), nameof(WalletId), IsUnique = true)]
    public class Category : NamedEntity
    {
        [Required]
        public bool IsIncome { get; set; }
        
        [Required]
        public int WalletId { get; set; }

        [ForeignKey(nameof(WalletId))]
        public virtual Wallet Wallet { get; set; }

        //public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
