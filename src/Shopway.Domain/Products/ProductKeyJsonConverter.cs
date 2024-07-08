using Shopway.Domain.Common.Utilities;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.EntityKeys;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public sealed class ProductKeyJsonConverter : JsonConverter<ProductKey>
{
    private const string ProductNameAsCamelCase = "productName";
    private const string RevisionAsCamelCase = "revision";

    public override ProductKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? productName = string.Empty;
        string? productRevision = string.Empty;

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                return ProductKey.Create(productName, productRevision);
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

            if (propertyName.Equals(nameof(ProductKey.ProductName), StringComparison.CurrentCultureIgnoreCase))
            {
                productName = reader.GetCurrentPropertyStringValue();
                continue;
            }

            if (propertyName.Equals(nameof(ProductKey.Revision), StringComparison.CurrentCultureIgnoreCase))
            {
                productRevision = reader.GetCurrentPropertyStringValue();
                continue;
            }

            throw new JsonException($"{nameof(ProductKey)} must only contain {nameof(ProductKey.ProductName)} and {nameof(ProductKey.Revision)}, but found '{propertyName}'");
        }

        throw new UnreachableException($"Reading {nameof(ProductKey)} unreachable exception.");
    }

    public override void Write(Utf8JsonWriter writer, ProductKey productKey, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(ProductNameAsCamelCase);
        writer.WriteStringValue(productKey.ProductName);
        writer.WritePropertyName(RevisionAsCamelCase);
        writer.WriteStringValue(productKey.Revision);
        writer.WriteEndObject();
    }
}
