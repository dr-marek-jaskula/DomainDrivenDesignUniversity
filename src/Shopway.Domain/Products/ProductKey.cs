using Shopway.Domain.Products;
using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.EntityKeys;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public readonly record struct ProductKey : IUniqueKey
{
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
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.