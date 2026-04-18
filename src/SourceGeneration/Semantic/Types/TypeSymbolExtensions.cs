using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Toarnbeike.SourceGeneration.Semantic.Display;

namespace Toarnbeike.SourceGeneration.Semantic.Types;

public static class TypeSymbolExtensions
{
    extension(INamedTypeSymbol type)
    {
        public ImmutableArray<IPropertySymbol> GetPublicPropertiesOrdered()
        {
            var syntaxReference = type.DeclaringSyntaxReferences;
            var syntax = syntaxReference.IsDefault
                ? null
                : syntaxReference.FirstOrDefault()?.GetSyntax() as TypeDeclarationSyntax;

            var propertySymbols = type
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public);

            if (syntax is null)
            {
                return propertySymbols.ToImmutableArray();
            }

            var propertyOrder = syntax.Members
                .OfType<PropertyDeclarationSyntax>()
                .Select((p, index) => (Name: p.Identifier.ValueText, Index: index))
                .ToDictionary(x => x.Name, x => x.Index);

            return propertySymbols
                .OrderBy(p => propertyOrder.TryGetValue(p.Name, out var index) ? index : int.MaxValue)
                .ToImmutableArray();
        }

        public bool ImplementsInterface(INamedTypeSymbol interfaceSymbol)
        {
            return type.AllInterfaces.Any(i =>
                SymbolEqualityComparer.Default.Equals(i, interfaceSymbol));
        }

        public bool InheritsFrom(INamedTypeSymbol baseType)
        {
            var current = type.BaseType;

            while (current is not null) // loop through all base types of the provided type
            {
                if (SymbolEqualityComparer.Default.Equals(current, baseType))
                {
                    return true;
                }

                if (current.OriginalDefinition is { } def && SymbolEqualityComparer.Default.Equals(def, baseType))
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }
    }

    public static string ToDefaultDisplayString(this ITypeSymbol type)
    {
        return type.ToDisplayString(SymbolDisplayFormats.FullyQualifiedType);
    }
}