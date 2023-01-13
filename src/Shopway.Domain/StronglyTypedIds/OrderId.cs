using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct OrderId : IEntityId<OrderId>
{
    private OrderId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static OrderId New()
    {
        return new OrderId(Guid.NewGuid());
    }

    public static OrderId Create(Guid id)
    {
        return new OrderId(id);
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