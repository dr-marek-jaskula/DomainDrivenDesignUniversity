using Shopway.Domain.Abstractions;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.EntityBusinessKeys;

public readonly record struct OrderLineKey : IBusinessKey
{
    public readonly OrderLineId OrderLineId { get; }

    public OrderLineKey(OrderLineId orderLineId)
    {
        OrderLineId = orderLineId;
    }

    public static OrderLineKey Create(OrderLineId orderLineId)
    {
        return new OrderLineKey(orderLineId);
    }

    public override string ToString()
    {
        return $"OrderLine {{ Id: {OrderLineId} }}";
    }
}
