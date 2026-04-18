using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Toarnbeike.SourceGeneration.Semantic.Types;

namespace Toarnbeike.SourceGeneration.Models.Factories;

public sealed class PropertyModelsFactory
{
    public static ImmutableArray<PropertyModel> Create(INamedTypeSymbol typeSymbol)
    {
        var orderedParameters = typeSymbol.GetPublicPropertiesOrdered();

        return orderedParameters
            .Select(PropertyModelFactory.Create)
            .ToImmutableArray();
    }
}