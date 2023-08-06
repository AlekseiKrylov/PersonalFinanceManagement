using PersonalFinanceManagement.Domain.DALEntities.Base;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DALEntities
{
    public class Wallet : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
