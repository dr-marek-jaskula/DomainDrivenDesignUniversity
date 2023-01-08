namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct OrderId : IEntityId<OrderId>, IEquatable<OrderId>
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

    public static OrderId New(Guid id)
    {
        return new OrderId(id);
    }

    public static bool operator ==(OrderId? first, OrderId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(OrderId? first, OrderId? second)
    {
        return !(first == second);
    }

    public bool Equals(OrderId? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Value.Value == Value;
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