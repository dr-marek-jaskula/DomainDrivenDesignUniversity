using Shopway.Domain.Abstractions;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.EntityKeys;

public readonly record struct OrderLineKey : IUniqueKey
{
    public readonly ProductId ProductId { get; }

    public OrderLineKey(ProductId productId)
    {
        ProductId = productId;
    }

    public static OrderLineKey Create(ProductId productId)
    {
        return new OrderLineKey(productId);
    }

    public override string ToString()
    {
        return $"OrderLine {{ Id: {ProductId} }}";
    }
}
