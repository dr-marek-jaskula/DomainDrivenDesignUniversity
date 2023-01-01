namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct RoleId : IEntityId<RoleId>
{
    private RoleId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static RoleId New()
    {
        return new RoleId(Guid.NewGuid());
    }

    public static RoleId New(Guid id)
    {
        return new RoleId(id);
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