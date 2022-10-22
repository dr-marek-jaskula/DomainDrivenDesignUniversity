using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

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
        Description description) 
        : base(id)
    {
        Priority = priority;
        Status = status;
        Title = title;
        Description = description;
    }

    // Empty constructor in this case is required by EF Core
    protected WorkItem()
    {
    }

    public int Priority { get; private set; }
    public Status Status { get; private set; }
    public string Title { get; private set; }
    public Description Description { get; private set; }
}