using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record OrderLineAddedDomainEvent(Guid Id, OrderLineId OrderLineId, OrderHeaderId OrderHeaderId) : DomainEvent(Id)
{
    public static OrderLineAddedDomainEvent New(OrderLineId OrderLineId, OrderHeaderId OrderHeaderId)
    {
        return new OrderLineAddedDomainEvent(Guid.NewGuid(), OrderLineId, OrderHeaderId);
    }
}