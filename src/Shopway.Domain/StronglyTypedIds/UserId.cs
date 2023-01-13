using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct UserId : IEntityId<UserId>, IEquatable<UserId>
{
    private UserId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId New(Guid id)
    {
        return new UserId(id);
    }

    public static bool operator ==(UserId? first, UserId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(UserId? first, UserId? second)
    {
        return !(first == second);
    }

    public bool Equals(UserId? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Value.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}