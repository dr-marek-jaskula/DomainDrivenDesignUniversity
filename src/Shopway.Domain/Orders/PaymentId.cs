using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Orders;

public readonly record struct PaymentId : IEntityId<PaymentId>
{
    private PaymentId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

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

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not PaymentId otherPaymentId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherPaymentId.Value);
    }

    public static bool operator >(PaymentId a, PaymentId b) => a.CompareTo(b) is 1;
    public static bool operator <(PaymentId a, PaymentId b) => a.CompareTo(b) is -1;
    public static bool operator >=(PaymentId a, PaymentId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(PaymentId a, PaymentId b) => a.CompareTo(b) <= 0;
}