using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct CustomerId : IEntityId<CustomerId>
{
    private CustomerId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static CustomerId New()
    {
        return new CustomerId(Guid.NewGuid());
    }

    public static CustomerId Create(Guid id)
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