namespace PersonalFinanceManagement.Domain.UIModels
{
    public class UserSession
    {
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
    }
}
