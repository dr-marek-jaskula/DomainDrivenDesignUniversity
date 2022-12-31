namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct WorkItemId : IEntityId
{
    private WorkItemId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static IEntityId Create(Guid id)
    {
        return new WorkItemId(id);
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