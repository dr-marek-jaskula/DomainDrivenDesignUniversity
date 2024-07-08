using Shopway.Domain.Common.Utilities;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Common.DataProcessing;

public sealed class FilterByEntryJsonConverter : JsonConverter<FilterByEntry>
{
    private readonly static JsonConverter<FilterByEntry.Predicate> _predicateValueConverter = (JsonConverter<FilterByEntry.Predicate>)JsonSerializerOptions.Default.GetConverter(typeof(FilterByEntry.Predicate));

    public override FilterByEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? predicatePropertyName = string.Empty;
        string? predicateOperation = string.Empty;
        object predicateValue = string.Empty;
        IList<FilterByEntry.Predicate> predicates = [];

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                if (predicates.Count is not 0)
                {
                    return new FilterByEntry(predicates);
                }

                return new FilterByEntry(predicatePropertyName, predicateOperation, predicateValue);
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

            if (propertyName.Equals(nameof(FilterByEntry.Predicates), StringComparison.CurrentCultureIgnoreCase))
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

                    if (reader.TokenType is not JsonTokenType.StartObject)
                    {
                        throw new JsonException("Should reach property name");
                    }

                    var predicate = _predicateValueConverter.Read(ref reader, typeof(FilterByEntry.Predicate), options)!;
                    predicates.Add(predicate);
                }

                continue;
            }

            if (propertyName.Equals(nameof(FilterByEntry.Predicate.PropertyName), StringComparison.CurrentCultureIgnoreCase))
            {
                predicatePropertyName = reader.GetCurrentPropertyStringValue();
                continue;
            }

            if (propertyName.Equals(nameof(FilterByEntry.Predicate.Operation), StringComparison.CurrentCultureIgnoreCase))
            {
                predicateOperation = reader.GetCurrentPropertyStringValue();
                continue;
            }

            if (propertyName.Equals(nameof(FilterByEntry.Predicate.Value), StringComparison.CurrentCultureIgnoreCase))
            {
                predicateValue = reader.GetCurrentPropertyValue();
                continue;
            }

            throw new JsonException($"{nameof(FilterByEntry.Predicate)}");
        }

        throw new UnreachableException($"Reading {nameof(FilterByEntry.Predicate)} unreachable exception.");
    }

    public override void Write(Utf8JsonWriter writer, FilterByEntry filterByEntry, JsonSerializerOptions options)
    {
        throw new UnreachableException($"{nameof(FilterByEntry)} should not be serialized to json");
    }


    public sealed class PredicateJsonConverter : JsonConverter<FilterByEntry.Predicate>
    {
        public override FilterByEntry.Predicate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? predicatePropertyName = string.Empty;
            string? predicateOperation = string.Empty;
            object predicateValue = string.Empty;

            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndObject)
                {
                    return new FilterByEntry.Predicate()
                    {
                        PropertyName = predicatePropertyName,
                        Operation = predicateOperation,
                        Value = predicateValue
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

                if (propertyName.Equals(nameof(FilterByEntry.Predicate.PropertyName), StringComparison.CurrentCultureIgnoreCase))
                {
                    predicatePropertyName = reader.GetCurrentPropertyStringValue();
                    continue;
                }

                if (propertyName.Equals(nameof(FilterByEntry.Predicate.Operation), StringComparison.CurrentCultureIgnoreCase))
                {
                    predicateOperation = reader.GetCurrentPropertyStringValue();
                    continue;
                }

                if (propertyName.Equals(nameof(FilterByEntry.Predicate.Value), StringComparison.CurrentCultureIgnoreCase))
                {
                    predicateValue = reader.GetCurrentPropertyValue();
                    continue;
                }

                throw new JsonException($"{nameof(FilterByEntry.Predicate)} must only contain {nameof(FilterByEntry.Predicate.PropertyName)},  {nameof(FilterByEntry.Predicate.Operation)}, {nameof(FilterByEntry.Predicate.Value)}, but found '{propertyName}'");
            }

            throw new UnreachableException($"Reading {nameof(FilterByEntry.Predicate)} unreachable exception.");
        }

        public override void Write(Utf8JsonWriter writer, FilterByEntry.Predicate predicate, JsonSerializerOptions options)
        {
            throw new UnreachableException($"{nameof(FilterByEntry.Predicate)} should not be serialized to json");
        }
    }
}
