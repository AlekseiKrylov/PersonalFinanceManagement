using PersonalFinanceManagement.Interfaces.Base.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.DAL.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
