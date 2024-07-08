using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Shopway.Domain.Common.DataProcessing;

public sealed class SortByEntryJsonConverter : JsonConverter<SortByEntry>
{
    public override SortByEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.String)
        {
            var value = reader.GetString()!;
            return GetSortByEntryFromString(value);
        }

        string? sortByEntryPropertyName = null;
        string? sortByEntrySortDirectionAsString = null;
        int? sortByEntrySortPriority = null;

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                return new SortByEntry()
                {
                    PropertyName = sortByEntryPropertyName!,
                    SortDirection = Enum.Parse<SortDirection>(sortByEntrySortDirectionAsString!),
                    SortPriority = (int)sortByEntrySortPriority!,
                    ParsedFromString = false
                };
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

            if (propertyName.Equals(nameof(SortByEntry.PropertyName), StringComparison.CurrentCultureIgnoreCase))
            {
                sortByEntryPropertyName = reader.GetCurrentPropertyStringValue();
                continue;
            }

            if (propertyName.Equals(nameof(SortByEntry.SortDirection), StringComparison.CurrentCultureIgnoreCase))
            {
                sortByEntrySortDirectionAsString = reader.GetCurrentPropertyStringValue();
                continue;
            }

            if (propertyName.Equals(nameof(SortByEntry.SortPriority), StringComparison.CurrentCultureIgnoreCase))
            {
                sortByEntrySortPriority = reader.GetCurrentPropertyIntValue();
                continue;
            }

            throw new JsonException($"{nameof(SortByEntry)}");
        }

        throw new UnreachableException($"Reading {nameof(SortByEntry)} unreachable exception.");
    }

    public override void Write(Utf8JsonWriter writer, SortByEntry sortByEntry, JsonSerializerOptions options)
    {
        throw new UnreachableException($"{nameof(SortByEntry)} should not be serialized to json");
    }

    private static SortByEntry GetSortByEntryFromString(string value)
    {
        if (value.NotContains(":"))
        {
            return new SortByEntry()
            {
                PropertyName = value,
                ParsedFromString = true
            };
        }

        var splitted = value
            .Split(':')
            .Select(x => x.Trim());

        return new SortByEntry()
        {
            PropertyName = splitted.First(),
            SortDirection = splitted.Last() == "-1"
                ? SortDirection.Descending
                : SortDirection.Ascending,
            ParsedFromString = true
        };
    }
}
