using System.Diagnostics;
using System.Text.Json;

namespace Shopway.Domain.Common.Utilities;

public static class Utf8JsonReaderUtilities
{
    public static string GetCurrentPropertyStringValue(this ref Utf8JsonReader reader)
    {
        reader.Read();
        var propertyValue = reader.GetString();

        if (propertyValue is null)
        {
            throw new JsonException($"Property value not found");
        }

        return propertyValue;
    }

    public static int GetCurrentPropertyIntValue(this ref Utf8JsonReader reader)
    {
        reader.Read();
        var propertyValue = reader.GetInt32();
        return propertyValue;
    }

    public static object GetCurrentPropertyValue(this ref Utf8JsonReader reader)
    {
        reader.Read();
        
        object propertyValue = reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt64(out long l) => l,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String when reader.TryGetDateTime(out DateTime datetime) => datetime,
            JsonTokenType.String => reader.GetString()!,
            _ => throw new UnreachableException()
        };

        if (propertyValue is null)
        {
            throw new JsonException($"Property value not found");
        }

        return propertyValue!;
    }
}
