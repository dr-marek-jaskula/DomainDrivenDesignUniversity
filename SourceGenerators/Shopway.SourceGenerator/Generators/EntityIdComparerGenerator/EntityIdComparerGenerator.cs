﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Shopway.SourceGenerator.Utilities;
using System.Collections.Immutable;
using System.Text;
using static Shopway.SourceGenerator.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class EntityIdComparerGenerator() : IncrementalGeneratorBase<EntityIdComparerToGenerateEntry>(RegisterSourceProvider, Generate)
{
    private const string GenerateEntityIdComparerAttributeMetadataName = "System.GenerateEntityIdComparerAttribute";
    private const string GenerateEntityIdComparerAttributeFileName = "GenerateEntityIdComparerAttribute.g.cs";

    public const string GenerateEntityIdComparerAttribute = Header + $$$"""

namespace System;

/// <summary>
/// Add to entity configurations to indicate that entity id comparer should be generated
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Generated by the dr-marek-jaskula source generator.")]
public class GenerateEntityIdComparerAttribute : global::System.Attribute
{
    public required string {{{IdName}}};
    public required string {{{IdNamespace}}};
}
""";

    private static IncrementalValuesProvider<EntityIdComparerToGenerateEntry> RegisterSourceProvider(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource
        (
            GenerateEntityIdComparerAttributeFileName,
            SourceText.From(GenerateEntityIdComparerAttribute, Encoding.UTF8))
        );

        return context
            .SyntaxProvider
            .ForAttributeWithMetadataName(GenerateEntityIdComparerAttributeMetadataName, Selectors.IsClass, MapToEntityIdComparerToGenerate)
            .Where(x => x != EntityIdComparerToGenerateEntry.None);
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<EntityIdComparerToGenerateEntry> entityIdComparerToGenerateEntries)
    {
        foreach (var entityIdComparerToGenerateEntry in entityIdComparerToGenerateEntries)
        {
            StringBuilder sb = new();
            var result = entityIdComparerToGenerateEntry.Generate(sb);
            context.AddSource(entityIdComparerToGenerateEntry.IdName + ".g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static EntityIdComparerToGenerateEntry MapToEntityIdComparerToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol entitySymbol)
        {
            return EntityIdComparerToGenerateEntry.None;
        }

        cancellationToken.ThrowIfCancellationRequested();

        AttributeData attribute = context.Attributes.First(a => GenerateEntityIdComparerAttributeMetadataName.Contains(a.AttributeClass?.Name));

        var idName = attribute.NamedArguments!.First(x => x.Key == IdName).Value.Value!.ToString();
        var idNamespace = attribute.NamedArguments!.First(x => x.Key == IdNamespace).Value.Value!.ToString();

        string @namespace = entitySymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : entitySymbol.ContainingNamespace.ToString();

        return new(idName, idNamespace, @namespace);
    }
}
