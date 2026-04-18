namespace Toarnbeike.SourceGeneration.Models;

public sealed record PropertyModel(string TypeName, string Name, bool IsReadOnly, bool HasDefaultValue, bool IsValueType);