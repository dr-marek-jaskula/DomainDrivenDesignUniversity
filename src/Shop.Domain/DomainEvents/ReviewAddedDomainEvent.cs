namespace Shopway.Domain.DomainEvents;

public sealed record ReviewAddedDomainEvent(Guid Id, Guid ReviewId, Guid ProductId) : DomainEvent(Id);