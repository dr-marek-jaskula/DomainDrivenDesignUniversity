namespace Shopway.Domain.DomainEvents;

public sealed record CustomerRegisteredDomainEvent(Guid Id, Guid UserId) : DomainEvent(Id);