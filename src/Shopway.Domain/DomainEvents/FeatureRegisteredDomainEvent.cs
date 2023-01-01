using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record FeatureRegisteredDomainEvent(Guid Id, WorkItemId FeatureId) : DomainEvent(Id);