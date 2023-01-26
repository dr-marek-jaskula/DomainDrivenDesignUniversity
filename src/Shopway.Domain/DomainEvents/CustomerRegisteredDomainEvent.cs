using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record CustomerRegisteredDomainEvent(Guid Id, PersonId CustomerId) : DomainEvent(Id)
{
    public static CustomerRegisteredDomainEvent New(PersonId customerId)
    {
        return new CustomerRegisteredDomainEvent(Guid.NewGuid(), customerId);
    }
}