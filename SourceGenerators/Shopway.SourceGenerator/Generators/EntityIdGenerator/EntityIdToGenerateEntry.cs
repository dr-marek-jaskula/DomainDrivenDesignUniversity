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
            .AppendLine()
            .AppendLine($"namespace {Namespace};")
            .AppendLine()
            .AppendLine($"public readonly record struct {Name} : IEntityId<{Name}>")
            .AppendLine($$$"""
            {
                private {{{Name}}}(Ulid id)
                {
                    Value = id;
                }

                public Ulid Value { get; }

                public static {{{Name}}} New()
                {
                    return new {{{Name}}}(Ulid.NewUlid());
                }

                public static {{{Name}}} Create(Ulid id)
                {
                    return new {{{Name}}}(id);
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

                public static bool operator >({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) is 1;
                public static bool operator <({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) is -1;
                public static bool operator >=({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) >= 0;
                public static bool operator <=({{{Name}}} a, {{{Name}}} b) => a.CompareTo(b) <= 0;
            }
            """)
            .ToString();
    }
}