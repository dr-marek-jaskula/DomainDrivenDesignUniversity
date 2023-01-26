using Shopway.Domain.EntityIds;
using System.ComponentModel;

namespace Shopway.Domain.Abstractions;

[TypeConverter(typeof(EntityIdConverter))]
public interface IEntityId<out TEntity> : IEntityId
{
    /// <summary>
    /// Create a new entity id using given guid
    /// </summary>
    public abstract static TEntity Create(Guid id);

    /// <summary>
    /// Create a new entity id using randomly generated guid
    /// </summary>
    public abstract static TEntity New();
}

public interface IEntityId
{
    public Guid Value { get; init; }
}