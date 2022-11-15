namespace Shopway.Domain.DomainEvents;

public sealed record BugRegisteredDomainEvent(Guid Id, Guid BugId) : DomainEvent(Id);