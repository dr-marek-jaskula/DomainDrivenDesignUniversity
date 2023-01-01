namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct UserId : IEntityId<UserId>
{
    private UserId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId New(Guid id)
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
}