using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistByEmailAsync(string email, CancellationToken cancel = default);
        Task<User> GetByEmailAsync(string email, CancellationToken cancel = default);
        Task<User> GetByVerificationTokenAsync(string verificationToken, CancellationToken cancel = default);
    }
}
