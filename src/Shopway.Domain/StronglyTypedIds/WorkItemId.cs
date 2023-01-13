using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct WorkItemId : IEntityId<WorkItemId>, IEquatable<WorkItemId>
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

    public static WorkItemId New(Guid id)
    {
        return new WorkItemId(id);
    }

    public static bool operator ==(WorkItemId? first, WorkItemId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(WorkItemId? first, WorkItemId? second)
    {
        return !(first == second);
    }

    public bool Equals(WorkItemId? other)
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