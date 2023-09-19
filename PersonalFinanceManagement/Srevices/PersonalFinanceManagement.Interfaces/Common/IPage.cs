namespace PersonalFinanceManagement.Interfaces.Common
{
    public interface IPage<T>
    {
        IEnumerable<T> Items { get; }
        int TotalItems { get; }
        int PageIndex { get; }
        int PageSize { get; }
        int TotalPagesCount { get; }
    }
}
