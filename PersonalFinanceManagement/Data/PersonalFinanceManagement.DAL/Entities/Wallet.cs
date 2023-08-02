using PersonalFinanceManagement.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.DAL.Entities
{
    public class Wallet : NamedEntity
    {
        [MaxLength(1000)]
        public string? Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        //public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}
