namespace Shopway.Domain.DomainEvents;

public sealed record UserCreatedDomainEvent(Guid Id, Guid UserId) : DomainEvent(Id);