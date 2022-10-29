using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities.Parents;

//Table-per-hierarchy approach (abstract class)

//The additional "Discriminator" column to distinguish the different children of a WorkItem
public abstract class WorkItem : AggregateRoot
{
    protected WorkItem
    (
        Guid id,
        Title title,
        Description description,
        Priority priority,
        StoryPoints storyPoints,
        Status status,
        Guid? employeeId
    )
        : base(id)
    {
        Priority = priority;
        Status = status;
        Title = title;
        Description = description;
        EmployeeId = employeeId;
        StoryPoints = storyPoints;
    }

    // Empty constructor in this case is required by EF Core
    protected WorkItem()
    {
    }

    public Priority Priority { get; private set; }
    public Status Status { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public StoryPoints StoryPoints { get; private set; }
    public Guid? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
}