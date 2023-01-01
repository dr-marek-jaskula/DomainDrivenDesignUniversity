namespace Shopway.Domain.StronglyTypedIds;

public interface IEntityId<out TEntity>
{
    public Guid Value { get; init; }

    /// <summary>
    /// Create a new entity id using given guid
    /// </summary>
    public abstract static TEntity New(Guid id);

    /// <summary>
    /// Create a new entity id using randomly generated guid
    /// </summary>
    public abstract static TEntity New();
}