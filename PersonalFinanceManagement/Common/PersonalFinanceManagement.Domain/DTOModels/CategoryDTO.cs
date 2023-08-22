using PersonalFinanceManagement.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class CategoryDTO : IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public bool IsIncome { get; set; }
        [Required]
        public int WalletId { get; set; }
    }
}
