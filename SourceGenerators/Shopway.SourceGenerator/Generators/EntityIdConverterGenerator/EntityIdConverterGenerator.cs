﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Shopway.SourceGenerator.Base;
using Shopway.SourceGenerator.Base.Utilities;
using System.Collections.Immutable;
using System.Text;
using static Shopway.SourceGenerator.Base.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class EntityIdConverterGenerator() : IncrementalGeneratorBase<EntityIdConverterToGenerateEntry>(RegisterSourceProvider, Generate)
{
    private const string GenerateEntityIdConverterAttributeMetadataName = "System.GenerateEntityIdConverterAttribute";
    private const string GenerateEntityIdConverterAttributeFileName = "GenerateEntityIdConverterAttribute.g.cs";

    public const string GenerateEntityIdConverterAttribute = Header + $$$"""

namespace System;

/// <summary>
/// Add to entity configurations to indicate that entity id converter should be generated
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Generated by the dr-marek-jaskula source generator.")]
public class GenerateEntityIdConverterAttribute : global::System.Attribute
{
    public required string {{{IdName}}};
    public required string {{{IdNamespace}}};
}
""";

    private static IncrementalValuesProvider<EntityIdConverterToGenerateEntry> RegisterSourceProvider(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource
        (
            GenerateEntityIdConverterAttributeFileName,
            SourceText.From(GenerateEntityIdConverterAttribute, Encoding.UTF8))
        );

        return context
            .SyntaxProvider
            .ForAttributeWithMetadataName(GenerateEntityIdConverterAttributeMetadataName, Selectors.IsClass, MapToEntityIdConverterToGenerate)
            .Where(x => x != EntityIdConverterToGenerateEntry.None);
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<EntityIdConverterToGenerateEntry> entityIdConverterToGenerateEntries)
    {
        foreach (var entityIdConverterToGenerateEntry in entityIdConverterToGenerateEntries)
        {
            StringBuilder sb = new();
            var result = entityIdConverterToGenerateEntry.Generate(sb);
            context.AddSource(entityIdConverterToGenerateEntry.IdName + GeneratedFileExtension, SourceText.From(result, Encoding.UTF8));
        }
    }

    private static EntityIdConverterToGenerateEntry MapToEntityIdConverterToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol entitySymbol)
        {
            return EntityIdConverterToGenerateEntry.None;
        }

        cancellationToken.ThrowIfCancellationRequested();

        AttributeData attribute = context.Attributes.First(a => GenerateEntityIdConverterAttributeMetadataName.Contains(a.AttributeClass?.Name));

        var idName = attribute.NamedArguments!.First(x => x.Key == IdName).Value.Value!.ToString();
        var idNamespace = attribute.NamedArguments!.First(x => x.Key == IdNamespace).Value.Value!.ToString();

        string @namespace = entitySymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : entitySymbol.ContainingNamespace.ToString();

        return new(idName, idNamespace, @namespace);
    }
}
