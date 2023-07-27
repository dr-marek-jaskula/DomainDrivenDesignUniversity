using Shopway.Domain.EntityIds;
using System.ComponentModel;

namespace Shopway.Domain.Abstractions;

[TypeConverter(typeof(EntityIdConverter))]
public interface IEntityId<out TEntityId> : IEntityId
{
    /// <summary>
    /// Create a new entity id using given ulid
    /// </summary>
    abstract static TEntityId Create(Ulid id);

    /// <summary>
    /// Create a new entity id using randomly generated ulid
    /// </summary>
    abstract static TEntityId New();
}

public interface IEntityId
{
    const string Id =  nameof(Id);
    Ulid Value { get; init; }
}