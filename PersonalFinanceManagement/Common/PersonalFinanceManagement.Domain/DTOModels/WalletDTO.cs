using PersonalFinanceManagement.Interfaces.Base.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class WalletDTO : IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
