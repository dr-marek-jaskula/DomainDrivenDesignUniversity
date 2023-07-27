using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct UserId : IEntityId<UserId>
{
    private UserId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static UserId New()
    {
        return new UserId(Ulid.NewUlid());
    }

    public static UserId Create(Ulid id)
    {
        return new UserId(id);
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