using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(PFMDbContext db) : base(db) { }

        public async Task<bool> ExistByEmailAsync(string email, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(u => u.Email == email, cancel).ConfigureAwait(false);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancel = default)
        {
            return await Items.SingleOrDefaultAsync(u => u.Email == email, cancel).ConfigureAwait(false);
        }
        
        public async Task<User> GetByVerificationTokenAsync(string verificationToken, CancellationToken cancel = default)
        {
            return await Items.SingleOrDefaultAsync(u => u.VerificationToken == verificationToken, cancel).ConfigureAwait(false);
        }
    }
}
