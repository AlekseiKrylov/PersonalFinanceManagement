using PersonalFinanceManagement.Domain.DALEntities.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalFinanceManagement.Domain.DALEntities
{
    public class User : Entity
    {
        [Required]
        public string Email { get; set; }

        [Required, JsonIgnore]
        public string PasswordHash { get; set; }

        public string VerificationToken { get; set; } = Guid.NewGuid().ToString();

        public DateTime? VerifiedAt { get; set; }

        public string? ResetPasswordToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
