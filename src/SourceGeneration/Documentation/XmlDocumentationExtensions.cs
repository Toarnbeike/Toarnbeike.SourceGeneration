using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Xml.Linq;
using Toarnbeike.SourceGeneration.Rendering;

namespace Toarnbeike.SourceGeneration.Documentation;

/// <summary>
/// Extensions to make working with Xml documentation easier.
/// </summary>
public static class XmlDocumentationExtensions
{
    /// <summary>
    /// Get the documentation for each parameter of the provided method.
    /// </summary>
    /// <param name="method">Method symbol to scan for xml documentation of parameters.</param>
    /// <returns>A dictionary containing the xml documentation as value per parameter as key.</returns>
    public static IReadOnlyDictionary<string, string> GetParameterDocumentation(
        this IMethodSymbol method)
    {
        var xml = method.GetDocumentationCommentXml(expandIncludes: true);
        if (string.IsNullOrWhiteSpace(xml))
        {
            return ImmutableDictionary<string, string>.Empty;
        }

        try
        {
            var doc = XDocument.Parse(xml);

            return doc.Descendants("param")
                .Where(e => e.Attribute("name") is not null)
                .ToDictionary(
                    e => e.Attribute("name")!.Value,
                    e => e.Value.ReplaceWhitespaceCharactersWithSpace());
        }
        catch
        {
            return ImmutableDictionary<string, string>.Empty;
        }
    }
}
