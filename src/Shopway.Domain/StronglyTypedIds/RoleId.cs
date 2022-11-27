namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct RoleId : IEntityId
{
    public RoleId()
    {
    }

    public Guid Value { get; init; }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}