using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityKeys;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public readonly record struct ReviewKey : IUniqueKey
{
    public readonly ProductKey ProductKey { get; }
    public readonly string Title { get; }

    public ReviewKey(ProductKey productKey, string title)
    {
        ProductKey = productKey;

        if (title is not null)
        {
            Title = NormalizeKeyComponent(title);
        }
    }

    public static ReviewKey Create(ProductKey productKey, string title)
    {
        return new ReviewKey(productKey, title);
    }

    private static string NormalizeKeyComponent(string keyComponent)
    {
        return keyComponent.Trim().ToLower();
    }

    public override string ToString()
    {
        return $"Review {{ Title: {Title}, {ProductKey} }}" ;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.