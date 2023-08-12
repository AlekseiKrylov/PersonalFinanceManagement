using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
