using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Toarnbeike.SourceGeneration.Semantic.Constructors;

public static class ConstructorExtensions
{
    /// <summary>
    /// Select the first constructor on this symbol that is
    /// - public
    /// - has more than one parameter
    /// - prefer one that is not implicitly declared
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns>The <see cref="IMethodSymbol"/> of the selected constructor, or none</returns>
    public static IMethodSymbol? SelectConstructor(this INamedTypeSymbol symbol)
    {
        var constructors = symbol.InstanceConstructors
            .Where(c => c.DeclaredAccessibility == Accessibility.Public).ToImmutableArray();

        return constructors.FirstOrDefault(c => !c.IsImplicitlyDeclared && c.Parameters.Length > 0)
               ?? constructors.FirstOrDefault(c => c.Parameters.Length > 0);
    }
}
