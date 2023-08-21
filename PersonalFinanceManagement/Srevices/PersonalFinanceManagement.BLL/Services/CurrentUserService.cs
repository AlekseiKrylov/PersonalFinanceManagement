using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.BLL.Exceptions;
using PersonalFinanceManagement.Interfaces.Services;

namespace PersonalFinanceManagement.BLL.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public int GetCurretUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User is not authorized.");

            if (!int.TryParse(user.FindFirst("id")?.Value, out int userId) && userId <= 0)
                throw new InvalidUserIdException("Invalid user ID.");

            return userId;
        }
    }
}
