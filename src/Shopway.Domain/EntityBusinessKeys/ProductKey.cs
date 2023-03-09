using Shopway.Domain.Abstractions;
namespace Shopway.Domain.EntityBusinessKeys;

public readonly record struct ProductKey : IBusinessKey
{
    public readonly string ProductName { get; }
    public readonly string Revision { get; }

    public ProductKey(string productName, string revision)
    {
        ProductName = NormalizeKeyComponent(productName);
        Revision = NormalizeKeyComponent(revision);
    }

    private static string NormalizeKeyComponent(string keyComponent)
    {
        return keyComponent.ToLower();
    }
}