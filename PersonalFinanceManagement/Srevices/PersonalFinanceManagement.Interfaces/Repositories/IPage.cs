namespace PersonalFinanceManagement.Interfaces.Base.Repositories
{
    public interface IPage<T>
    {
        IEnumerable<T> Items { get;  }

        int TotalCount { get; }
        
        int PageIndex { get; }
        
        int PageSize { get; }

        int TotalPagesCount { get; }
    }
}
