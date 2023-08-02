using PersonalFinanceManagement.Interfaces.Base.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.DAL.Entities.Base
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
