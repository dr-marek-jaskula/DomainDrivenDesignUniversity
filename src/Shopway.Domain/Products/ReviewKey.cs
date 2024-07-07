using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.EntityKeys;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public sealed class ReviewKeyJsonConverter : JsonConverter<ReviewKey>
{
    private const string TitleAsCamelCase = "title";

    public override ReviewKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? title = string.Empty;

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                return ReviewKey.Create(title);
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

            if (propertyName.Equals(nameof(ReviewKey.Title), StringComparison.CurrentCultureIgnoreCase))
            {
                title = reader.GetCurrentPropertyValue();
                continue;
            }

            throw new JsonException($"{nameof(ReviewKey)} must only contain {nameof(ReviewKey.Title)}, but found '{propertyName}'");
        }

        throw new UnreachableException($"Reading {nameof(ReviewKey)} unreachable exception.");
    }

    public override void Write(Utf8JsonWriter writer, ReviewKey reviewKey, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(TitleAsCamelCase);
        writer.WriteStringValue(reviewKey.Title);
        writer.WriteEndObject();
    }
}

