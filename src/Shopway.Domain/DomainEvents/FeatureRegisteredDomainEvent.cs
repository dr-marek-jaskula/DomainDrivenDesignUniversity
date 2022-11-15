namespace Shopway.Domain.DomainEvents;

public sealed record FeatureRegisteredDomainEvent(Guid Id, Guid FeatureId) : DomainEvent(Id);