namespace Toarnbeike.SourceGeneration.Rendering;

public static class NamingExtensions
{
    public static string ToCamelCase(this string name)
    {
        return string.IsNullOrEmpty(name) || char.IsLower(name[0])
            ? name
            : name.Length == 1
                ? name.ToLower()
                : char.ToLower(name[0]) + name.Substring(1);
    }

    public static string ToPrivateFieldName(this string name)
    {
        return string.IsNullOrEmpty(name)
            ? name
            : $"_{name.ToCamelCase()}";
    }

    public static string ToNullableType(this string type)
    {
        if (string.IsNullOrEmpty(type) || type.EndsWith("?"))
        {
            return type;
        }

        if (type.StartsWith("global::Toarnbeike.Optional.Option<"))
        {
            return type.Substring(35, type.Length - 36) + "?";
        }

        return type + "?";
    }
}