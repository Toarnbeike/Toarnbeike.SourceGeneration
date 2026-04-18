using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Toarnbeike.SourceGeneration.Semantic.Properties;

public static class PropertyExtensions
{
    extension(IPropertySymbol propertySymbol)
    {
        public bool HasDefaultValue()
        {
            return propertySymbol.DeclaringSyntaxReferences
                .Select(r => r.GetSyntax())
                .OfType<PropertyDeclarationSyntax>()
                .Any(p => p.Initializer is not null);
        }
    }
}
