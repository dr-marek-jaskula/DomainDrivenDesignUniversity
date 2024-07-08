using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Text.Json.Serialization;

namespace Shopway.Domain.EntityKeys;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[JsonConverter(typeof(ReviewKeyJsonConverter))]
public readonly record struct ReviewKey : IUniqueKey
{
    public readonly string Title { get; }

    public ReviewKey(string title)
    {
        if (title is not null)
        {
            Title = NormalizeKeyComponent(title);
        }
    }

    public static ReviewKey Create(string title)
    {
        return new ReviewKey(title);
    }

    private static string NormalizeKeyComponent(string keyComponent)
    {
        return keyComponent.Trim().ToLower();
    }

    public override string ToString()
    {
        return $"Review {{ Title: {Title} }}";
    }
}
