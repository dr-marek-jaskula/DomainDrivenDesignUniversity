using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Products;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.EntityKeys;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[JsonConverter(typeof(ProductKeyJsonConverter))]
public readonly record struct ProductKey : IUniqueKey<Product, ProductKey>
{
    private const string NormalizedProductName = "productname";
    private const string NormalizedRevision = "revision";
    private const int ComponentsCount = 2;

    public readonly string ProductName { get; init; }
    public readonly string Revision { get; init; }

    public ProductKey(string productName, string revision)
    {
        if (productName is not null)
        {
            ProductName = NormalizeKeyComponent(productName);
        }

        if (revision is not null)
        {
            Revision = NormalizeKeyComponent(revision);
        }
    }

    public static ProductKey Create(string productName, string revision)
    {
        return new ProductKey(productName, revision);
    }

    private static string NormalizeKeyComponent(string keyComponent)
    {
        return keyComponent.Trim().ToLower();
    }

    public override string ToString()
    {
        return $"Product {{ Name: {ProductName}, Revision: {Revision} }}";
    }

    public static ProductKey From(Product product)
    {
        return new ProductKey(product.ProductName.Value, product.Revision.Value);
    }

    public Expression<Func<Product, bool>> CreateQuerySpecification()
    {
        var name = ProductName;
        var revision = Revision;

        return product => (string)(object)product.ProductName == name
                       && (string)(object)product.Revision == revision;
    }

    public static ProductKey Create(Dictionary<string, string> key)
    {
        var normalizedKey = key.ToDictionary(x => NormalizeKeyComponent(x.Key), x => x.Value);

        var getProductNameResult = normalizedKey.TryGetValue(NormalizedProductName, out var productName);
        var getRevisionResult = normalizedKey.TryGetValue(NormalizedRevision, out var revision);

        if (getProductNameResult is false || getRevisionResult is false || key.Count is not ComponentsCount)
        {
            throw new ArgumentException($"Current: {string.Join(", ", key.Keys)}. Valid: {nameof(ProductName)}, {nameof(Revision)}");
        }

        return Create(productName!, revision!);
    }
}

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
