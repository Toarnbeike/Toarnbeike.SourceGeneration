using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Toarnbeike.SourceGeneration.Documentation;
using Toarnbeike.SourceGeneration.Semantic.Constructors;

namespace Toarnbeike.SourceGeneration.Models.Factories;

public sealed class ConstructorParameterModelsFactory
{
    public static ImmutableArray<ConstructorParameterModel> Create(INamedTypeSymbol symbol)
    {
        var constructor = symbol.SelectConstructor();
        if (constructor is null)
        {
            return [];
        }

        var documentation = constructor.GetParameterDocumentation();

        return constructor.Parameters
            .Select(p => ConstructorParameterModelFactory
                .Create(p, documentation.TryGetValue(p.Name, out var docs) ? docs : null))
            .ToImmutableArray();
    }
}