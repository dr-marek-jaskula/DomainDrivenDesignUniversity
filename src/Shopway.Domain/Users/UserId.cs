using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Domain.Users;

[DebuggerDisplay("{Value}")]
[JsonConverter(typeof(UserIdJsonConverter))]
public readonly record struct UserId : IEntityId<UserId>, IParsable<UserId>
{
    private UserId(Ulid id)
    {
        Value = id;
    }

    public readonly Ulid Value { get; init; }

    public static UserId New()
    {
        return new UserId(Ulid.NewUlid());
    }
    
    public static UserId Create(Ulid id)
    {
        return new UserId(id);
    }
    
    public static UserId Create(string id)
    {
        if (Ulid.TryParse(id, out var ulid))
        {
            return Create(ulid);
        }

        throw new InvalidOperationException($"'{id}' cannot be parsed to Ulid");
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not UserId otherUserId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherUserId.Value);
    }

    public static UserId Parse(string entityIdAsString, IFormatProvider? provider)
    {
        return Create(entityIdAsString);
    }

    public static bool TryParse([NotNullWhen(true)] string? entityIdAsString, IFormatProvider? provider, [MaybeNullWhen(false)] out UserId result)
    {
        if (entityIdAsString is null)
        {
            throw new InvalidOperationException($"'{entityIdAsString}' cannot be null");
        }

        result = Create(entityIdAsString);
        return true;
    }

    public static bool operator >(UserId a, UserId b) => a.CompareTo(b) is 1;
    public static bool operator <(UserId a, UserId b) => a.CompareTo(b) is -1;
    public static bool operator >=(UserId a, UserId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(UserId a, UserId b) => a.CompareTo(b) <= 0;
}

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
