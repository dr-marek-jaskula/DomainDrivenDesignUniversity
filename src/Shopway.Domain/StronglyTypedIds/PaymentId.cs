using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PaymentId : IEntityId<PaymentId>
{
    private PaymentId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static PaymentId New()
    {
        return new PaymentId(Guid.NewGuid());
    }

    public static PaymentId Create(Guid id)
    {
        return new PaymentId(id);
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