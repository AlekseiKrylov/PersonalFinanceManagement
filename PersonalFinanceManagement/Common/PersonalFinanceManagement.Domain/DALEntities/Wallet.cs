using PersonalFinanceManagement.Domain.DALEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Domain.DALEntities
{
    public class Wallet : Entity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
