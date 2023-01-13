using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PaymentId : IEntityId<PaymentId>, IEquatable<PaymentId>
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

    public static PaymentId New(Guid id)
    {
        return new PaymentId(id);
    }

    public static bool operator ==(PaymentId? first, PaymentId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(PaymentId? first, PaymentId? second)
    {
        return !(first == second);
    }

    public bool Equals(PaymentId? other)
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