using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Orders;

public readonly record struct OrderLineId : IEntityId<OrderLineId>
{
    private OrderLineId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

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

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not OrderLineId otherOrderLineId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherOrderLineId.Value);
    }

    public static bool operator >(OrderLineId a, OrderLineId b) => a.CompareTo(b) is 1;
    public static bool operator <(OrderLineId a, OrderLineId b) => a.CompareTo(b) is -1;
    public static bool operator >=(OrderLineId a, OrderLineId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(OrderLineId a, OrderLineId b) => a.CompareTo(b) <= 0;
}