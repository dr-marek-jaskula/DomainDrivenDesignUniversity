using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.BaseTypes;

public abstract class Entity<TEntityId> : IEntity
    where TEntityId : struct, IEntityId<TEntityId>
{
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public TEntityId Id { get; private init; }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}