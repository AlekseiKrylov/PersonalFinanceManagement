using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.BLLModels
{
    public class UserLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
