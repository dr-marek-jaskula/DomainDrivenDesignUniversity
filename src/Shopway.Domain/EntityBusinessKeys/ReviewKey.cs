using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityBusinessKeys;

//BusinessKeys for not aggregates can contain keys, because they are only queried as a part of aggregate

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public readonly record struct ReviewKey : IBusinessKey
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