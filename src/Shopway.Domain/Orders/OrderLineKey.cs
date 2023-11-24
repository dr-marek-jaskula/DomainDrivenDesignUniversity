using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Products;

namespace Shopway.Domain.Orders;

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
