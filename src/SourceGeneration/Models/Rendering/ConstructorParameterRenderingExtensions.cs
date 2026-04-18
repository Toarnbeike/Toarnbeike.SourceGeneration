using System.Collections.Immutable;
using Toarnbeike.SourceGeneration.Naming;
using Toarnbeike.SourceGeneration.Rendering.Text;

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

public static class PropertyModelsRenderingExtensions
{
    extension(ImmutableArray<PropertyModel> properties)
    {
        public string GetAsParameterList(bool includeDefault = false)
        {
            var segments = new List<string>();

            segments.AddRange(properties
                .Where(p => !p.IsReadOnly && !p.HasDefaultValue)
                .Select(p => $"{p.TypeName} {p.Name.ToCamelCase()}"));

            if (includeDefault)
            {
                segments.AddRange(properties
                    .Where(p => !p.IsReadOnly && p.HasDefaultValue)
                    .Select(p => $"{p.TypeName.ToNullableType()} {p.Name.ToCamelCase()} = null"));
            }

            return segments.JoinCommaSeparated();
        }

        public string GetAsAssignments(bool includeDefault = false, int indentLevel = 2)
        {
            var segments = new List<string>();

            segments.AddRange(properties
                .Where(p => !p.IsReadOnly && !p.HasDefaultValue)
                .Select(p => $"{p.Name} = {p.Name.ToCamelCase()}"));

            if (includeDefault)
            {

            }

            return segments.JoinLines().Indent(indentLevel);
        }
    }
}