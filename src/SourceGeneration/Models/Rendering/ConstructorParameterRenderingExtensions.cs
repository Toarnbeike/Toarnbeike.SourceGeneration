using System.Collections.Immutable;
using Toarnbeike.SourceGeneration.Rendering;

namespace Toarnbeike.SourceGeneration.Models.Rendering;

public static class ConstructorParameterRenderingExtensions
{
    extension(ImmutableArray<ConstructorParameterModel> parameters)
    {
        public string GetXmlComments() =>
            parameters
                .Select(param =>
                    $"""
                     /// <param name="{param.Name.ToCamelCase()}">{param.XmlDescription ?? $"{param.Name} ({param.TypeName})"}</param>
                     """)
                .JoinLines();

        public string GetAsParameters() =>
            parameters
                .Select(param => $"{param.TypeName} {param.Name.ToCamelCase()}")
                .JoinCommaSeparated();

        public string GetAsArguments() =>
            parameters
                .Select(param => param.Name.ToCamelCase())
                .JoinCommaSeparated();
    }
}