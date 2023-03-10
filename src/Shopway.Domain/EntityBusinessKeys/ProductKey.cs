using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityBusinessKeys;

public readonly record struct ProductKey : IBusinessKey
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
}