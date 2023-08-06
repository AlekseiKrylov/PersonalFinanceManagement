namespace PersonalFinanceManagement.Domain.ModelsDTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome { get; set; }
        public int WalletId { get; set; }
    }
}
