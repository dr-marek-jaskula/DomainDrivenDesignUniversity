using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct OrderLineId : IEntityId<OrderLineId>
{
    private OrderLineId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static OrderLineId New()
    {
        return new OrderLineId(Ulid.NewUlid());
    }

    public static OrderLineId Create(Ulid id)
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