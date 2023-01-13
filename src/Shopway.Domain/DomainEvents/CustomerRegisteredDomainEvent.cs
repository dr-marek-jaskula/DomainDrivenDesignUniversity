using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record CustomerRegisteredDomainEvent(Guid Id, PersonId CustomerId) : DomainEvent(Id)
{
    public static CustomerRegisteredDomainEvent New(PersonId customerId)
    {
        return new CustomerRegisteredDomainEvent(Guid.NewGuid(), customerId);
    }
}