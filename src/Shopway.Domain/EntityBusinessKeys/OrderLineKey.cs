using Shopway.Domain.Abstractions;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.EntityBusinessKeys;

public readonly record struct OrderLineKey : IBusinessKey
{
    public readonly OrderHeaderId OrderHeaderId { get; }
    public readonly ProductId ProductId { get; }

    public OrderLineKey(OrderHeaderId orderHeaderId, ProductId productId)
    {
        ProductId = productId;
        OrderHeaderId = orderHeaderId;
    }

    public static OrderLineKey Create(OrderHeaderId orderHeaderId, ProductId productId)
    {
        return new OrderLineKey(orderHeaderId, productId);
    }

    public override string ToString()
    {
        return $"OrderLine {{ Id: {ProductId} }}";
    }
}
