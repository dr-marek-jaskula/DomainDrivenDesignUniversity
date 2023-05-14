using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct OrderHeaderId : IEntityId<OrderHeaderId>
{
    private OrderHeaderId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static OrderHeaderId New()
    {
        return new OrderHeaderId(Guid.NewGuid());
    }

    public static OrderHeaderId Create(Guid id)
    {
        return new OrderHeaderId(id);
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