namespace Shopway.Domain.DomainEvents;

public sealed record OrderCreatedDomainEvent(Guid Id, Guid OrderId) : DomainEvent(Id);
