using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record BugRegisteredDomainEvent(Guid Id, WorkItemId BugId) : DomainEvent(Id)
{
    public static BugRegisteredDomainEvent New(WorkItemId BugId)
    {
        return new BugRegisteredDomainEvent(Guid.NewGuid(), BugId);
    }
}