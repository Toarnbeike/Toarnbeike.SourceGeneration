using System.Collections.Immutable;
using Toarnbeike.SourceGeneration.Rendering;

namespace Toarnbeike.SourceGeneration.Models.Rendering;

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