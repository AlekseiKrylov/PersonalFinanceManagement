using PersonalFinanceManagement.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class TransactionCreateDTO : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string? Note { get; set; }
        [Required, Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
