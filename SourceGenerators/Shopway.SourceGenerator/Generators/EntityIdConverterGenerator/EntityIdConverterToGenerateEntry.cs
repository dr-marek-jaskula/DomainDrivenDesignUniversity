using System.Text;
using static Shopway.SourceGenerator.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

public readonly record struct EntityIdConverterToGenerateEntry
{
    public readonly string IdName;
    public readonly string IdNamespace;
    public readonly string Namespace;

    public static readonly EntityIdConverterToGenerateEntry None = new(string.Empty, string.Empty, string.Empty);

    public EntityIdConverterToGenerateEntry
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
            .AppendLine($"using Microsoft.EntityFrameworkCore.Storage.ValueConversion;")
            .AppendLine($"using {IdNamespace};")
            .AppendLine()
            .AppendLine($"namespace {Namespace};")
            .AppendLine()
            .AppendLine($"public sealed class {IdName}Converter : ValueConverter<{IdName}, string>")
            .AppendLine($$$"""
            {
                public {{{IdName}}}Converter() : base(id => id.Value.ToString(), ulid => {{{IdName}}}.Create(Ulid.Parse(ulid))) { }
            }
            """)
            .ToString();
    }
}