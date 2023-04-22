using Shopway.Domain.Abstractions;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.EntityBusinessKeys;

//BusinessKeys for not aggregates can contain ids, because they are only queried as a part of aggregate

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public readonly record struct ReviewKey : IBusinessKey
{
    public readonly ProductId ProductId { get; }
    public readonly string Title { get; }

    public ReviewKey(ProductId productId, string title)
    {
        ProductId = productId;

        if (title is not null)
        {
            Title = NormalizeKeyComponent(title);
        }
    }

    public static ReviewKey Create(ProductId productId, string title)
    {
        return new ReviewKey(productId, title);
    }

    private static string NormalizeKeyComponent(string keyComponent)
    {
        return keyComponent.Trim().ToLower();
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.