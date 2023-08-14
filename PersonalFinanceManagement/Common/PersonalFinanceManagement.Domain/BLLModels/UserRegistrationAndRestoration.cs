using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.BLLModels
{
    public class UserRegistrationAndRestoration
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
