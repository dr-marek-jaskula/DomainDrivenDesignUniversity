using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Users;

public readonly record struct CustomerId : IEntityId<CustomerId>
{
    private CustomerId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

    public static CustomerId New()
    {
        return new CustomerId(Ulid.NewUlid());
    }

    public static CustomerId Create(Ulid id)
    {
        return new CustomerId(id);
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

        if (other is not CustomerId otherCustomerId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherCustomerId.Value);
    }

    public static bool operator >(CustomerId a, CustomerId b) => a.CompareTo(b) is 1;
    public static bool operator <(CustomerId a, CustomerId b) => a.CompareTo(b) is -1;
    public static bool operator >=(CustomerId a, CustomerId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(CustomerId a, CustomerId b) => a.CompareTo(b) <= 0;
}