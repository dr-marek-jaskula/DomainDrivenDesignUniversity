using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Users;

public readonly record struct UserId : IEntityId<UserId>
{
    private UserId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

    public static UserId New()
    {
        return new UserId(Ulid.NewUlid());
    }

    public static UserId Create(Ulid id)
    {
        return new UserId(id);
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

        if (other is not UserId otherUserId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherUserId.Value);
    }

    public static bool operator >(UserId a, UserId b) => a.CompareTo(b) is 1;
    public static bool operator <(UserId a, UserId b) => a.CompareTo(b) is -1;
    public static bool operator >=(UserId a, UserId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(UserId a, UserId b) => a.CompareTo(b) <= 0;
}