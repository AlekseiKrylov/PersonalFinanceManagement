using PersonalFinanceManagement.Interfaces.Common;
using PersonalFinanceManagement.Interfaces.Entities;

namespace PersonalFinanceManagement.Domain.UIModels
{
    public record PageItems<T>(IEnumerable<T> Items, int TotalItems, int PageIndex, int PageSize) : IPage<T> where T : IEntity
    {
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalItems / PageSize);
    };
}
