using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct OrderLineId : IEntityId<OrderLineId>
{
    private OrderLineId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static OrderLineId New()
    {
        return new OrderLineId(Guid.NewGuid());
    }

    public static OrderLineId Create(Guid id)
    {
        return new OrderLineId(id);
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