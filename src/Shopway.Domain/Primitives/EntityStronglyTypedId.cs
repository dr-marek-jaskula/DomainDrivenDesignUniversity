namespace Shopway.Domain.Primitives;

public abstract class EntityStronglyTypedId<TStonglyTypedId> : IEquatable<EntityStronglyTypedId<TStonglyTypedId>>
    where TStonglyTypedId : StronglyTypedId
{
    protected EntityStronglyTypedId(Guid id)
    {
        Id = id;
    }

    protected EntityStronglyTypedId()
    {
    }

    public TStonglyTypedId Id { get; private init; }

    public static bool operator ==(EntityStronglyTypedId<TStonglyTypedId>? first, EntityStronglyTypedId<TStonglyTypedId>? second)
    {
        return first is not null 
            && second is not null 
            && first.Equals(second);
    }

    public static bool operator !=(EntityStronglyTypedId<TStonglyTypedId>? first, EntityStronglyTypedId<TStonglyTypedId>? second)
    {
        return !(first == second);
    }

    public bool Equals(EntityStronglyTypedId<TStonglyTypedId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id.Value == Id.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not IStronglyTypedId entity)
        {
            return false;
        }

        return entity.Value == Id.Value;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}