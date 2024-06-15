using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Shopway.SourceGenerator.Utilities;

public static class Transformations
{
    public static string ClassToDisplayStringOrEmpty(GeneratorSyntaxContext generatorSyntaxContext, CancellationToken cancellationToken)
    {
        var declaredSymbol = generatorSyntaxContext
            .SemanticModel
            .GetDeclaredSymbol((ClassDeclarationSyntax)generatorSyntaxContext.Node, cancellationToken);

        return declaredSymbol is not null
            ? declaredSymbol.ToDisplayString()
            : string.Empty;
    }
}
