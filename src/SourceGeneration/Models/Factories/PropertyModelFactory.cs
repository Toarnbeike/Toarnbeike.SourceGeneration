using Microsoft.CodeAnalysis;
using Toarnbeike.SourceGeneration.Semantic.Properties;
using Toarnbeike.SourceGeneration.Semantic.Types;

namespace Toarnbeike.SourceGeneration.Models.Factories;

public sealed class PropertyModelFactory
{
    public static PropertyModel Create(IPropertySymbol propertySymbol)
    {
        var typeName = propertySymbol.Type.ToDefaultDisplayString();
        var isReadOnly = propertySymbol.IsReadOnly;
        var hasDefaultValue = propertySymbol.HasDefaultValue();
        var isValueType = propertySymbol.Type.IsValueType;

        return new PropertyModel(typeName, propertySymbol.Name, isReadOnly, hasDefaultValue, isValueType);
    }
}