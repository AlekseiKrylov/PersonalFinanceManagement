using PersonalFinanceManagement.Interfaces.Base.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class WalletCreateDTO : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
