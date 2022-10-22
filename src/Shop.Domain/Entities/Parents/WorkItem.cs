using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities.Parents;

//Table-per-hierarchy approach (abstract class)

//The additional "Discriminator" column to distinguish the different children of a WorkItem
public abstract class WorkItem : AggregateRoot
{
    protected WorkItem(
        Guid id, 
        int priority, 
        Status status, 
        string title, 
        string description) 
        : base(id)
    {
        Priority = priority;
        Status = status;
        Title = title;
        Description = description;
    }

    public int Priority { get; private set; }
    public Status Status { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
}