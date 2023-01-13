using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record OrderCreatedDomainEvent(Guid Id, OrderId OrderId) : DomainEvent(Id)
{
    public static OrderCreatedDomainEvent New(OrderId OrderId)
    {
        return new OrderCreatedDomainEvent(Guid.NewGuid(), OrderId);
    }
}
