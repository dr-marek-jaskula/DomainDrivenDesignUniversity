using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Products;
using System.Linq.Expressions;

namespace Shopway.Domain.EntityKeys;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public readonly record struct ProductKey : IUniqueKey<Product, ProductKey>
{
    private const string NormalizedProductName = "productname";
    private const string NormalizedRevision = "revision";
    private const int ComponentsCount = 2;

    public readonly string ProductName { get; }
    public readonly string Revision { get; }

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

    public Expression<Func<Product, bool>> FindSpecification() 
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

        if (getProductNameResult is false || getRevisionResult is false || key.Count != ComponentsCount)
        {
            throw new ArgumentException($"Current: {string.Join(", ", key.Keys)}. Valid: {nameof(ProductName)}, {nameof(Revision)}");
        }

        return Create(productName!, revision!);
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
