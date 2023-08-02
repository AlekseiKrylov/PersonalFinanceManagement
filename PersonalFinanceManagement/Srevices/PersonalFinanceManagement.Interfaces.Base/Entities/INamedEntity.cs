namespace PersonalFinanceManagement.Interfaces.Base.Entities
{
    public interface INamedEntity : IEntity
    {
        string Name { get; }
    }
}
