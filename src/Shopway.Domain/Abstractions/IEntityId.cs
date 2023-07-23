using Shopway.Domain.EntityIds;
using System.ComponentModel;

namespace Shopway.Domain.Abstractions;

[TypeConverter(typeof(EntityIdConverter))]
public interface IEntityId<out TEntityId> : IEntityId
{
    /// <summary>
    /// Create a new entity id using given guid
    /// </summary>
    abstract static TEntityId Create(Guid id);

    /// <summary>
    /// Create a new entity id using randomly generated guid
    /// </summary>
    abstract static TEntityId New();
}

public interface IEntityId
{
    const string Id =  nameof(Id);
    Guid Value { get; init; }
}