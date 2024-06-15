using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Shopway.SourceGenerator.Utilities;

public static class Selectors
{
    public static bool IsClass(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is ClassDeclarationSyntax;
    }

    public static bool IsMethod(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is MethodDeclarationSyntax;
    }

    public static bool IsProperty(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is PropertyDeclarationSyntax;
    }

    public static bool IsField(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode is FieldDeclarationSyntax;
    }

    public static bool IsEnum(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is EnumDeclarationSyntax;
    }

    public static bool IsEnumWithAttribute(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is EnumDeclarationSyntax @enum && @enum.AttributeLists.Count > 0;
    }
}
