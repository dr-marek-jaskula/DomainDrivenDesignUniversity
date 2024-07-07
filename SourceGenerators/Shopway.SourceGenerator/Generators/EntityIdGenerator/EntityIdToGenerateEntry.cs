using System.Text;
using static Shopway.SourceGenerator.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

public readonly record struct EntityIdToGenerateEntry
{
    /// <summary>
    /// Due to the fact that this generators is created for particular application and the namespace where generic IEntityId interface will not change it is hardcoded
    /// If for some reason the generic EntityId namespace would change, than it should be adjusted.
    /// More generic approach would be to accept an namespace at EntityIdAttribute as string or to introduce generic parameter for IEntityId on EntityIdAttribute.
    /// That would allow to read the namespace where generic IEntityId is. 
    /// </summary>
    private const string GenericIEntityIdNamespace = "using Shopway.Domain.Common.BaseTypes.Abstractions;";
    private const string DiagnosticsNamespace = "using System.Diagnostics;";
    private const string DiagnosticCodeAnalysisNamespace = "using System.Diagnostics.CodeAnalysis;";
    private const string TestJsonNamespace = "using System.Text.Json;";
    private const string TestJsonSerializationNamespace = "using System.Text.Json.Serialization;";

    public readonly string Name;
    public readonly string Namespace;

    public static readonly EntityIdToGenerateEntry None = new(string.Empty, string.Empty);

    public EntityIdToGenerateEntry
    (
        string name,
        string @namespace
    )
    {
        Name = name;
        Namespace = @namespace;
    }

    public string Generate(StringBuilder stringBuilder)
    {
        return stringBuilder
            .Append(Header)
            .AppendLine()
            .AppendLine(GenericIEntityIdNamespace)
            .AppendLine(DiagnosticsNamespace)
            .AppendLine(DiagnosticCodeAnalysisNamespace)
            .AppendLine(TestJsonNamespace)
            .AppendLine(TestJsonSerializationNamespace)
            .AppendLine()
            .AppendLine($"namespace {Namespace};")
            .AppendLine()
            .AppendLine($$$"""[DebuggerDisplay("{Value}")]""")
            .AppendLine($$$"""[JsonConverter(typeof({{{Name}}}JsonConverter))]""")
            .AppendLine($"public readonly record struct {Name} : IEntityId<{Name}>, IParsable<{Name}>")
            .AppendLine($$$"""
            {
                public const string Name = "{{{Name}}}";
                public const string Namespace = "{{{Namespace}}}";

                private {{{Name}}}(Ulid id)
                {
                    Value = id;
                }

                public readonly Ulid Value { get; init; }

                public static {{{Name}}} New()
                {
                    return new {{{Name}}}(Ulid.NewUlid());
                }

                public static {{{Name}}} Create(Ulid id)
                {
                    return new {{{Name}}}(id);
                }

                public static {{{Name}}} Create(string id)
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

                    if (other is not {{{Name}}} otherId)
                    {
                        throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
                    }

                    return Value.CompareTo(otherId.Value);
                }

                public static {{{Name}}} Parse(string entityIdAsString, IFormatProvider? provider)
                {
                    return Create(entityIdAsString);
                }

                public static bool TryParse([NotNullWhen(true)] string? entityIdAsString, IFormatProvider? provider, [MaybeNullWhen(false)] out {{{Name}}} result)
                {
                    if (entityIdAsString is null)
                    {
                        throw new InvalidOperationException($"'{entityIdAsString}' cannot be null");
                    }

                    result = Create(entityIdAsString);
                    return true;
                }

                public static bool operator >({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) is 1;
                public static bool operator <({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) is -1;
                public static bool operator >=({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) >= 0;
                public static bool operator <=({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) <= 0;
            }

            public sealed class {{{Name}}}JsonConverter : JsonConverter<{{{Name}}}>
            {
                public override {{{Name}}} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    var entityIdAsString = reader.GetString();

                    if (Ulid.TryParse(entityIdAsString, out var ulid))
                    {
                        return {{{Name}}}.Create(ulid);
                    }

                    throw new InvalidOperationException($"'{entityIdAsString}' cannot be parsed to Ulid");
                }

                public override void Write(Utf8JsonWriter writer, {{{Name}}} entityId, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(entityId.Value.ToString());
                }
            }
            """)
            .ToString();
    }
}
