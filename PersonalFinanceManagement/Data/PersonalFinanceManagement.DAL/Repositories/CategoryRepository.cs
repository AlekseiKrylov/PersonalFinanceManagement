using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Repository;

namespace PersonalFinanceManagement.DAL.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private int _userId;

        protected override IQueryable<Category> Items => _userId > 0 ? Set.Where(c => c.Wallet.UserId == _userId) : Set;

        public CategoryRepository(PFMDbContext db) : base(db) { }

        public void SetUserId(int userId) => _userId = userId;
    }
}
