using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record ProductCreatedDomainEvent(Guid Id, ProductId ProductId) : DomainEvent(Id);