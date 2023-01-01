namespace Shopway.Domain.StronglyTypedIds;

public interface IEntityId<out TEntity>
{
    public Guid Value { get; init; }
    public abstract static TEntity New(Guid id);
    public abstract static TEntity New();
}