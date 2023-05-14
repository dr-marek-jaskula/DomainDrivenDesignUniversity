using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record CustomerRegisteredDomainEvent(Guid Id, CustomerId CustomerId) : DomainEvent(Id)
{
    public static CustomerRegisteredDomainEvent New(CustomerId customerId)
    {
        return new CustomerRegisteredDomainEvent(Guid.NewGuid(), customerId);
    }
}