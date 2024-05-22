using System.Text;
using static Shopway.SourceGenerator.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

public readonly record struct EnumConverterToGenerateEntry
{
    public readonly string EnumName;
    public readonly string EnumNamespace;
    public readonly string Namespace;

    public static readonly EnumConverterToGenerateEntry None = new(string.Empty, string.Empty, string.Empty);

    public EnumConverterToGenerateEntry
    (
        string enumName,
        string enumNamespace,
        string @namespace
    )
    {
        EnumName = enumName;
        EnumNamespace = enumNamespace;
        Namespace = @namespace;
    }

    public string Generate(StringBuilder stringBuilder)
    {
        return stringBuilder
            .Append(Header)
            .AppendLine()
            .AppendLine($"using Microsoft.EntityFrameworkCore.Storage.ValueConversion;")
            .AppendLine($"using {EnumNamespace};")
            .AppendLine()
            .AppendLine($"namespace {Namespace};")
            .AppendLine()
            .AppendLine($"public sealed class {EnumName}Converter : ValueConverter<{EnumName}, string>")
            .AppendLine($$$"""
            {
                public {{{EnumName}}}Converter() : base(status => status.ToString(), @string => ({{{EnumName}}})Enum.Parse(typeof({{{EnumName}}}), @string)) { }
            }
            """)
            .ToString();
    }
}