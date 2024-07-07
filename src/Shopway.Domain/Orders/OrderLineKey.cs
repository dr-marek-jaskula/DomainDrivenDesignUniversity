using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Products;
using System.Text.Json;
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

public sealed class OrderLineKeyJsonConverter : JsonConverter<OrderLineKey>
{
    public override OrderLineKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var entityIdAsString = reader.GetString();

        if (Ulid.TryParse(entityIdAsString, out var ulid))
        {
            return OrderLineKey.Create(ProductId.Create(ulid));
        }

        throw new InvalidOperationException($"'{entityIdAsString}' cannot be parsed to Ulid");
    }

    public override void Write(Utf8JsonWriter writer, OrderLineKey orderLineKey, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteStringValue(orderLineKey.ToString());
        writer.WriteEndObject();
    }
}
