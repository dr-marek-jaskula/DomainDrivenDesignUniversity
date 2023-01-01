using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record CustomerRegisteredDomainEvent(Guid Id, PersonId CustomerId) : DomainEvent(Id);