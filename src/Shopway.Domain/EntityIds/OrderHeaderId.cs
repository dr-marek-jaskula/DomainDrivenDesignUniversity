using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct OrderHeaderId : IEntityId<OrderHeaderId>
{
    private OrderHeaderId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

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

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not OrderHeaderId otherOrderHeaderId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherOrderHeaderId.Value);
    }

    public static bool operator >(OrderHeaderId a, OrderHeaderId b) => a.CompareTo(b) is 1;
    public static bool operator <(OrderHeaderId a, OrderHeaderId b) => a.CompareTo(b) is -1;
    public static bool operator >=(OrderHeaderId a, OrderHeaderId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(OrderHeaderId a, OrderHeaderId b) => a.CompareTo(b) <= 0;
}