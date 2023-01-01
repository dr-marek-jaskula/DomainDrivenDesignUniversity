using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record BugRegisteredDomainEvent(Guid Id, WorkItemId BugId) : DomainEvent(Id);