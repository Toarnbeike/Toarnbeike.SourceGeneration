using Microsoft.CodeAnalysis;
using Toarnbeike.SourceGeneration.Semantic.Types;

namespace Toarnbeike.SourceGeneration.Models.Factories;

public sealed class ConstructorParameterModelFactory
{
    public static ConstructorParameterModel Create(IParameterSymbol parameterSymbol, string? docs)
    {
        var typeName = parameterSymbol.Type.ToDefaultDisplayString();
        return new ConstructorParameterModel(typeName, parameterSymbol.Name, docs);
    }
}