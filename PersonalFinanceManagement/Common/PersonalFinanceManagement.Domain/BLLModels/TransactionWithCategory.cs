namespace PersonalFinanceManagement.Domain.BLLModels
{
    public class TransactionWithCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryIsIncome { get; set; }
    }
}
