using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record OrderCreatedDomainEvent(Guid Id, OrderId OrderId) : DomainEvent(Id);
