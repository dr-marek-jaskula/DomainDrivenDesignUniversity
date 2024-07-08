using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Users;

public sealed class UserIdJsonConverter : JsonConverter<UserId>
{
    public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var entityIdAsString = reader.GetString();

        if (Ulid.TryParse(entityIdAsString, out var ulid))
        {
            return UserId.Create(ulid);
        }

        throw new InvalidOperationException($"'{entityIdAsString}' cannot be parsed to Ulid");
    }

    public override void Write(Utf8JsonWriter writer, UserId entityId, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteStringValue(entityId.Value.ToString());
        writer.WriteEndObject();
    }
}
