using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PersonalFinanceManagement.Domain.DTOModels;

namespace PersonalFinanceManagement.Domain.UIModels
{
    public class CreateTransactionForm
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string? Note { get; set; }
        [Required, Column(TypeName = "decimal(18, 2)")]
        public decimal? Amount { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? Date { get; set; } = DateTime.Today;
        [Required]
        public CategoryDTO Category { get; set; }
    }
}
