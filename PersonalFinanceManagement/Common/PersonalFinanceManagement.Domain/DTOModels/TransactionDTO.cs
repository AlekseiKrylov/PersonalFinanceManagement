using PersonalFinanceManagement.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class TransactionDTO : IEntity
    {
        [Required]
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

        public override bool Equals(object? obj)
        {
            var other = obj as TransactionDTO;
            return other?.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Name is null ? string.Empty : Name;
    }
}
