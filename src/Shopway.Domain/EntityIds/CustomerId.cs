using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct CustomerId : IEntityId<CustomerId>
{
    private CustomerId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static CustomerId New()
    {
        return new CustomerId(Ulid.NewUlid());
    }

    public static CustomerId Create(Ulid id)
    {
        return new CustomerId(id);
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