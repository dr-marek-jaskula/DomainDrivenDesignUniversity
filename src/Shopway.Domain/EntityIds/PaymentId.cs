using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct PaymentId : IEntityId<PaymentId>
{
    private PaymentId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static PaymentId New()
    {
        return new PaymentId(Ulid.NewUlid());
    }

    public static PaymentId Create(Ulid id)
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