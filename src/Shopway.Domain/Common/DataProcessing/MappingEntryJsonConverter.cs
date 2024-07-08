using Shopway.Domain.Common.Utilities;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Common.DataProcessing;

public sealed class MappingEntryJsonConverter : JsonConverter<MappingEntry>
{
    public override MappingEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.String)
        {
            return new MappingEntry()
            {
                PropertyName = reader.GetString(),
            };
        }

        string? mappingEntryFrom = null;
        List<MappingEntry>? mappingEntryProperties = [];

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                return new MappingEntry()
                {
                    PropertyName = null!,
                    From = mappingEntryFrom,
                    Properties = mappingEntryProperties
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

            if (propertyName.Equals(nameof(MappingEntry.From), StringComparison.CurrentCultureIgnoreCase))
            {
                mappingEntryFrom = reader.GetCurrentPropertyStringValue();
                continue;
            }

            if (propertyName.Equals(nameof(MappingEntry.Properties), StringComparison.CurrentCultureIgnoreCase))
            {
                reader.Read();

                if (reader.TokenType is not JsonTokenType.StartArray)
                {
                    throw new JsonException("Should reach property name");
                }

                while (reader.Read())
                {
                    if (reader.TokenType is JsonTokenType.EndArray)
                    {
                        break;
                    }

                    if (reader.TokenType is not JsonTokenType.String and not JsonTokenType.StartObject)
                    {
                        throw new JsonException("Should reach property name");
                    }

                    var mappingEntry = Read(ref reader, typeToConvert, options)!;
                    mappingEntryProperties.Add(mappingEntry);
                }

                continue;
            }

            throw new JsonException($"{nameof(MappingEntry)}");
        }

        throw new UnreachableException($"Reading {nameof(MappingEntry)} unreachable exception.");
    }

    public override void Write(Utf8JsonWriter writer, MappingEntry sortByEntry, JsonSerializerOptions options)
    {
        throw new UnreachableException($"{nameof(MappingEntry)} should not be serialized to json");
    }
}
