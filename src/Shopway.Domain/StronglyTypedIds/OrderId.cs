namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct OrderId : IEntityId
{
    private OrderId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static IEntityId Create(Guid id)
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