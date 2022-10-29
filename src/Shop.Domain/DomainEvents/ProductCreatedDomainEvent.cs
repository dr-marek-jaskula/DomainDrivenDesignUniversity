namespace Shopway.Domain.DomainEvents;

public sealed record ProductCreatedDomainEvent(Guid Id, Guid ProductId) : DomainEvent(Id);