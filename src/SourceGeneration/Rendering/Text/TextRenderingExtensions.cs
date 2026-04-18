using System.Text;
// ReSharper disable ConvertToExtensionBlock -- name of arguments are valuable and different.

namespace Toarnbeike.SourceGeneration.Rendering.Text;

public static class TextRenderingExtensions
{
    /// <summary>
    /// Join a set of lines by placing them on new lines.
    /// Utility function to make pipelines nicer.
    /// </summary>
    /// <param name="lines">The lines to place.</param>
    public static string JoinLines(this IEnumerable<string> lines) => string.Join("\n", lines);

    /// <summary>
    /// Join a set of values by placing them comma separated
    /// </summary>
    /// <param name="values">The values to separate.</param>
    public static string JoinCommaSeparated(this IEnumerable<string> values) => string.Join(", ", values);

    /// <summary>
    /// Reformats a string by replacing whitespace characters with a space.
    /// </summary>
    internal static string ReplaceWhitespaceCharactersWithSpace(this string text) =>
        string.Join(" ", text.Split(['\r', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries));

    /// <summary>
    /// Indent a string (supports multi line) by the provided level.
    /// </summary>
    /// <param name="text">The text to indent</param>
    /// <param name="level">The indentation level</param>
    /// <param name="indent">The string by which to indent, defaults to 4 spaces </param>
    /// <returns>The indented string.</returns>
    public static string Indent(this string text, int level, string indent = "    ")
    {
        if (string.IsNullOrEmpty(text) || level <= 0 || string.IsNullOrEmpty(indent))
        {
            return text;
        }

        var indentLen = indent.Length * level;

        var lines = 1 + text.Count(c => c == '\n');

        var sb = new StringBuilder(text.Length + lines * indentLen);

        WriteIndent(level, sb, indent);

        foreach (var c in text)
        {
            sb.Append(c);
            if (c == '\n')
            {
                WriteIndent(level, sb, indent);
            }
        }

        return sb.ToString();
    }

    private static void WriteIndent(int level, StringBuilder sb, string indent)
    {
        for (var i = 0; i < level; i++)
        {
            sb.Append(indent);
        }
    }
}
