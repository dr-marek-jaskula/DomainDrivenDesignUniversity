using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Bug : WorkItem
{
    private Bug(
        WorkItemId id,
        Title title,
        Description description,
        Priority priority,
        StoryPoints storyPoints,
        Status status,
        Guid? employeeId)
        : base(id, title, description, priority, storyPoints, status, employeeId)
    {
    }

    // Empty constructor in this case is required by EF Core
    private Bug()
    {
    }

    public static Bug Create(
        WorkItemId id,
        Title title,
        Description description,
        Priority priority,
        StoryPoints storyPoints,
        Status status,
        Guid? employeeId)
    {
        var bug = new Bug(
            id,
            title,
            description,
            priority,
            storyPoints,
            status,
            employeeId);

        bug.RaiseDomainEvent(BugRegisteredDomainEvent.New(bug.Id));

        return bug;
    }
}