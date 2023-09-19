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

        public override bool Equals(object? obj)
        {
            var other = obj as CategoryDTO;
            return other?.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Name is null ? string.Empty : Name;
    }
}
