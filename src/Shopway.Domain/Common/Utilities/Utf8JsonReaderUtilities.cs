using System.Text.Json;

namespace Shopway.Domain.Common.Utilities;

public static class Utf8JsonReaderUtilities
{
    public static string GetCurrentPropertyValue(this ref Utf8JsonReader reader)
    {
        reader.Read();
        var propertyValue = reader.GetString();

        if (propertyValue is null)
        {
            throw new JsonException($"Property value not found");
        }

        return propertyValue;
    }
}
