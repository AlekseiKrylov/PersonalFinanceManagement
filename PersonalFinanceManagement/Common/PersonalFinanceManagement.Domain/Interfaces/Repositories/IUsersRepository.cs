using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<bool> ExistByEmailAsync(string email, CancellationToken cancel = default);
        Task<User> GetByEmailAsync(string email, CancellationToken cancel = default);
        Task<User> GetByVerificationTokenAsync(string verificationToken, CancellationToken cancel = default);
    }
}
