using PersonalFinanceManagement.Interfaces.Base.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DALEntities.Base
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
