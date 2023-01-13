using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct WorkItemId : IEntityId<WorkItemId>
{
    private WorkItemId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static WorkItemId New()
    {
        return new WorkItemId(Guid.NewGuid());
    }

    public static WorkItemId Create(Guid id)
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