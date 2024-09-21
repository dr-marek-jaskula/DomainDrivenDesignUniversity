using System.Text;
using static Shopway.SourceGenerator.Base.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

public readonly record struct EntityIdComparerToGenerateEntry
{
    public readonly string IdName;
    public readonly string IdNamespace;
    public readonly string Namespace;

    public static readonly EntityIdComparerToGenerateEntry None = new(string.Empty, string.Empty, string.Empty);

    public EntityIdComparerToGenerateEntry
    (
        string idName,
        string idNamespace,
        string @namespace
    )
    {
        IdName = idName;
        IdNamespace = idNamespace;
        Namespace = @namespace;
    }

    public string Generate(StringBuilder stringBuilder)
    {
        return stringBuilder
            .Append(Header)
            .AppendLine()
            .AppendLine($"using Microsoft.EntityFrameworkCore.ChangeTracking;")
            .AppendLine($"using {IdNamespace};")
            .AppendLine()
            .AppendLine($"namespace {Namespace};")
            .AppendLine()
            .AppendLine($"public sealed class {IdName}Comparer : ValueComparer<{IdName}>")
            .AppendLine($$$"""
            {
                public {{{IdName}}}Comparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
            }
            """)
            .ToString();
    }
}
