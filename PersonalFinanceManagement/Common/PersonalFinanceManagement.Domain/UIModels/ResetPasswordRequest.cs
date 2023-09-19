using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.UIModels
{
    public class ResetPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetPasswordToken { get; set; }
    }
}
