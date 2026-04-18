# Toarnbeike.SourceGeneration

`Toarnbeike.SourceGeneration` is a utility library for building incremental source generators.
It provides reusable abstractions for working with Roslyn, focused on improving readability,
correctness, and composability of generator pipelines.

> This library is currently intended for internal use and experimentation.

## Design philosophy

The library follows the natural stages of an incremental source generator:

`Discovery -> Selection -> Semantics -> Modeling -> Rendering`

Each namespace in this library maps to one of these concerns.

The goal is to:
- reduce boilerplate
- enforce correct Roslyn usage (symbol-based, incremental-safe)
- improve readability of generator code

## Selection

Selection is the entry point of the incremental pipeline.

Instead of manually writing `CreateSyntaxProvider` logic, the library provides
higher-level abstractions for common patterns.

### Example

```csharp
var types = context.ForAttribute<MyAttribute>();
```

### Available

- ForAttribute<TAttribute>
  - Finds all types annotated with TAttribute
  - Handles syntax filtering + semantic validation

### Planned

- ForInterface<TInterface>
- ForBaseType<TBase>

## Semantics

Semantic helpers operate on Roslyn symbols (`ISymbol`, `INamedTypeSymbol`, etc.).

These extensions aim to:
- replace string-based matching with symbol-based matching
- simplify attribute and type inspection
- provide consistent formatting

The following extensions are provided:

### Attributes
- AttributeDataExtensions - get data supplied by the attribute
- AttributeSymbolExtensions - check if symbol is annotated with attribute, and/or return attribute data

### Types
- display helpers
- todo: interface checks

## Models
Common models are provided by the library:
- `ConstructorParameterModel(string TypeName, string Name, string? XmlDescription)` with rendering options.

## Rendering
Rendering helpers assist in generating source code strings.

These are intentionally simple primitives, not a templating system.

### Available
- UsingBuilder - build `using` statements from provided types
- TextRenderingExtensions
  - JoinLines
  - JoinCommaSeparatedLines
  - Indent

### Notes
Rendering operates on
- strings
- Roslyn symbols (preferred over raw strings)

## Naming
Helpers for consistent naming conventions.
- `ToCamelCase()`

### Documentation
Helpers for reading XML documentation from symbols.
- XmlDocumentationExtensions
  - extract `<param>` documentation from methods

### Utilities
Low-level helpers that are not provided by `netstandard2.0`.
- HashCodeHelper