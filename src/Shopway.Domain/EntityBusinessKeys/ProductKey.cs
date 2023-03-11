using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityBusinessKeys;

public readonly record struct ProductKey : IBusinessKey
{
    public readonly string ProductName { get; }
    public readonly string Revision { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ProductKey(string productName, string revision)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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