using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

namespace PersonalFinanceManagement.Domain.Interfaces.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void SetUserId(int userId);
    }
}
