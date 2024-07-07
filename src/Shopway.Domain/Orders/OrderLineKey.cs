using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using System.Diagnostics;
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
    private const string ProductIdAsCamelCase = "productId";

    public override OrderLineKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? productIdAsString = string.Empty;

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                if (Ulid.TryParse(productIdAsString, out var ulid))
                {
                    return OrderLineKey.Create(ProductId.Create(ulid));
                }

                throw new InvalidOperationException($"'{productIdAsString}' cannot be parsed to Ulid");
            }

            if (reader.TokenType is not JsonTokenType.PropertyName)
            {
                throw new JsonException("Should reach property name");
            }

            string? propertyName = reader.GetString();

            if (propertyName is null)
            {
                throw new JsonException("Did not reach EndObject");
            }

            if (propertyName.Equals(nameof(OrderLineKey.ProductId), StringComparison.CurrentCultureIgnoreCase))
            {
                productIdAsString = reader.GetCurrentPropertyValue();
                continue;
            }

            throw new JsonException($"{nameof(ReviewKey)} must only contain {nameof(ReviewKey.Title)}, but found '{propertyName}'");
        }

        throw new UnreachableException($"Reading {nameof(ReviewKey)} unreachable exception.");
    }

    public override void Write(Utf8JsonWriter writer, OrderLineKey orderLineKey, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(ProductIdAsCamelCase);
        writer.WriteStringValue(orderLineKey.ProductId.ToString());
        writer.WriteEndObject();
    }
}
