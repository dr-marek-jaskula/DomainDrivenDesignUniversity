﻿using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using Shopway.SourceGenerator.Utilities;
using System.Collections.Immutable;
using System.Text;
using static Shopway.SourceGenerator.Utilities.Constants;

namespace Shopway.SourceGenerator.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class EntityIdGenerator() : IncrementalGeneratorBase<EntityIdToGenerateEntry>(RegisterSourceProvider, Generate)
{
    private const string GenerateEntityIdAttributeMetadataName = "System.GenerateEntityIdAttribute";
    private const string GenerateEntityIdAttributeFileName = "GenerateEntityIdAttribute.g.cs";

    public const string GenerateEntityIdAttribute = Header + """

namespace System;

/// <summary>
/// Add to entities to indicate that entity id structure should be generated
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Generated by the dr-marek-jaskula source generator.")]
public class GenerateEntityIdAttribute : global::System.Attribute;
""";

    private static IncrementalValuesProvider<EntityIdToGenerateEntry> RegisterSourceProvider(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource
        (
            GenerateEntityIdAttributeFileName,
            SourceText.From(GenerateEntityIdAttribute, Encoding.UTF8))
        );

        return context
            .SyntaxProvider
            .ForAttributeWithMetadataName(GenerateEntityIdAttributeMetadataName, Selectors.IsClass, MapToEntityIdToGenerate)
            .Where(x => x != EntityIdToGenerateEntry.None);
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<EntityIdToGenerateEntry> entityIdToGenerateEntries)
    {
        foreach (var entityIdToGenerateEntry in entityIdToGenerateEntries)
        {
            StringBuilder sb = new();
            var result = entityIdToGenerateEntry.Generate(sb);
            context.AddSource(entityIdToGenerateEntry.Name + ".g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static EntityIdToGenerateEntry MapToEntityIdToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol entitySymbol)
        {
            return EntityIdToGenerateEntry.None;
        }

        cancellationToken.ThrowIfCancellationRequested();

        string name = entitySymbol.Name + "Id";
        string @namespace = entitySymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : entitySymbol.ContainingNamespace.ToString();

        return new(name, @namespace);
    }
}