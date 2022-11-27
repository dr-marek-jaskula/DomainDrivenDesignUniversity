namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct WorkItemId : IEntityId
{
    public WorkItemId(Guid id)
    {
        Value = id;
    }

    public WorkItemId()
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