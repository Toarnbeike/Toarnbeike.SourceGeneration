using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Toarnbeike.SourceGeneration.Semantic.Attributes;
using Toarnbeike.SourceGeneration.Semantic.Types;

namespace Toarnbeike.SourceGeneration.Selection;

public static class IncrementalSelectionExtensions
{
    /// <param name="context">The full Generation context.</param>
    extension(IncrementalGeneratorInitializationContext context)
    {
        /// <summary>
        /// Provide the <see cref="INamedTypeSymbol"/> for the type requested.
        /// </summary>
        /// <typeparam name="TType">The type to provide the type symbol for.</typeparam>
        public IncrementalValueProvider<INamedTypeSymbol?> ForType<TType>()
        {
            return context.CompilationProvider
                .Select((c, _) => c.GetTypeByMetadataName(typeof(TType).FullName!));
        }

        /// <summary>
        /// Select all types as <see cref="INamedTypeSymbol"/>
        /// </summary>
        public IncrementalValuesProvider<INamedTypeSymbol> ForTypes()
        {
            return context.SyntaxProvider
                .CreateSyntaxProvider(
                    static (node, _) => node is TypeDeclarationSyntax,
                    static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as INamedTypeSymbol)
                .WhereNotNull();
        }

        /// <summary>
        /// Provide all <see cref="INamedTypeSymbol"/> in the context that are annotated with the <typeparamref name="TAttribute"/> Attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute to select.</typeparam>
        /// <returns>An <see cref="IncrementalValuesProvider{TValue}"/> containing all values that are annotated with the attribute.</returns>
        public IncrementalValuesProvider<INamedTypeSymbol> ForAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            var attributeSymbol = context.ForType<TAttribute>();

            return context.ForTypes()
                .CombineWith(attributeSymbol)
                .Where(static t => t.Right is not null)
                .Where(t => t.Left.HasAttribute(t.Right!))
                .Select(t => t.Left);
        }

        /// <summary>
        /// Provide all <see cref="INamedTypeSymbol"/> in the context that implement the <typeparamref name="TInterface"/> interface.
        /// Already accounts for: - indirect implementation, generic interfaces and multiple interfaces per type.
        /// </summary>
        /// <typeparam name="TInterface">The interface to select.</typeparam>
        /// <returns>An <see cref="IncrementalValuesProvider{TValue}"/> containing all values that implement the interface.</returns>
        public IncrementalValuesProvider<INamedTypeSymbol> ForInterface<TInterface>()
        {
            var interfaceSymbol = context.ForType<TInterface>();

            return context.ForTypes()
                .CombineWith(interfaceSymbol)
                .Where(static t => t.Right is not null)
                .Where(t => t.Left.ImplementsInterface(t.Right!))
                .Select(t => t.Left);
        }

        /// <summary>
        /// Provide all <see cref="INamedTypeSymbol"/> in the context that inherit the <typeparamref name="TBase"/> type.
        /// Already accounts for: - indirect inheritance.
        /// </summary>
        /// <typeparam name="TBase">The base to select.</typeparam>
        /// <returns>An <see cref="IncrementalValuesProvider{TValue}"/> containing all values that inherit from the base type.</returns>
        public IncrementalValuesProvider<INamedTypeSymbol> ForBaseType<TBase>()
        {
            var interfaceSymbol = context.ForType<TBase>();

            return context.ForTypes()
                .CombineWith(interfaceSymbol)
                .Where(static t => t.Right is not null)
                .Where(t => t.Left.InheritsFrom(t.Right!))
                .Select(t => t.Left);
        }
    }
}
