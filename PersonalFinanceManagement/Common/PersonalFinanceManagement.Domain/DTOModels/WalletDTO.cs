using PersonalFinanceManagement.Interfaces.Base.Entities;

namespace PersonalFinanceManagement.Domain.ModelsDTO
{
    public class WalletDTO : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
