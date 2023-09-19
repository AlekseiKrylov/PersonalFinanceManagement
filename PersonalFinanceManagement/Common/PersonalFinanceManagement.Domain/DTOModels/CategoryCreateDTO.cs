using PersonalFinanceManagement.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class CategoryCreateDTO : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public bool IsIncome { get; set; }
        [Required]
        public int WalletId { get; set; }
    }
}
