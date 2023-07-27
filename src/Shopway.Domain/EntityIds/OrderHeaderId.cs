using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct OrderHeaderId : IEntityId<OrderHeaderId>
{
    private OrderHeaderId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static OrderHeaderId New()
    {
        return new OrderHeaderId(Ulid.NewUlid());
    }

    public static OrderHeaderId Create(Ulid id)
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