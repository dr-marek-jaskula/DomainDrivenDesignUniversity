using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using Shopway.SourceGenerator.Utilities;
using System.Collections.Immutable;
using System.Text;

namespace Shopway.SourceGenerator.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class EntityIdGenerator() : IncrementalGeneratorBase<EntityIdToGenerateEntry>(RegisterSourceProvider, Generate)
{
    private const string EntityIdAttribute = "System.EntityIdAttribute";
    private const string EntityIdAttributeFileName = "EntityIdAttribute.g.cs";

    private static IncrementalValuesProvider<EntityIdToGenerateEntry> RegisterSourceProvider(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource
        (
            EntityIdAttributeFileName,
            SourceText.From(EntityIdGeneratorUtilities.EntityIdAttribute, Encoding.UTF8))
        );

        return context
            .SyntaxProvider
            .ForAttributeWithMetadataName(EntityIdAttribute, Selectors.IsClass, MapToEntityIdToGenerate)
            .Where(x => x != EntityIdToGenerateEntry.None);
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<EntityIdToGenerateEntry> displayValues)
    {
        foreach (var displayValue in displayValues)
        {
            StringBuilder sb = new();
            var result = EntityIdGeneratorUtilities.GenerateEntityIdStruct(sb, displayValue);
            context.AddSource(displayValue.Name + ".g.cs", SourceText.From(result, Encoding.UTF8));
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