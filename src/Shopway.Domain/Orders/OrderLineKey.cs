using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Products;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Orders;

[JsonConverter(typeof(OrderLineKeyJsonConverter))]
public readonly record struct OrderLineKey : IUniqueKey
{
    public readonly ProductId ProductId { get; init; }

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
        return $"OrderLine {{ ProductId: {ProductId} }}";
    }
}
