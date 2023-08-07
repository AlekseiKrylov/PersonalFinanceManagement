using PersonalFinanceManagement.Interfaces.Base.Entities;

namespace PersonalFinanceManagement.Domain.ModelsDTO
{
    public class TransactionDTO : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int WalletId { get; set; }
    }
}
