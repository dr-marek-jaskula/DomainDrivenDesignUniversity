namespace Shopway.Domain.DomainEvents;

public sealed record OrderRegisteredDomainEvent(Guid Id, Guid OrderId) : DomainEvent(Id);
