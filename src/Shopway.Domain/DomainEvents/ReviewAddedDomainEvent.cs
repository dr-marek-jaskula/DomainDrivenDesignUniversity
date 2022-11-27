using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record ReviewAddedDomainEvent(Guid Id, ReviewId ReviewId, ProductId ProductId) : DomainEvent(Id);