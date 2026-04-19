using Microsoft.CodeAnalysis;

namespace Toarnbeike.SourceGeneration.Rendering.Usings;

public static class UsingBuilder
{
    /// <summary>
    /// Generate using statements for the provided type, optionally excluding the one namespace (typically the namespace of the creating type).
    /// </summary>
    /// <param name="types">The types as <see cref="ITypeSymbol"/> to create using statements for</param>
    /// <param name="excludeNamespace">The namespace to exclude, typically the namespace of the creating type.</param>
    /// <returns>Left aligned string with using statements, separated by a newline.</returns>
    public static string FromTypes(
        IEnumerable<ITypeSymbol> types,
        string? excludeNamespace = null)
    {
        return types.Select(type => type.ContainingNamespace)
                .Where(ns => ns is not null)
                .Select(type => type.ToDisplayString())
                .Except([excludeNamespace])
                .Distinct()
                .OrderBy(ns => ns)
                .Select(ns => $"using {ns};")
                .JoinLines();
    }
}
