using Microsoft.CodeAnalysis;

namespace Toarnbeike.SourceGeneration.Selection;

public static class IncrementalGeneratorBuildingBlocks
{
    public static IncrementalValuesProvider<T> WhereNotNull<T>(this IncrementalValuesProvider<T?> source) where T : class
    {
        return source
            .Where(static x => x is not null)
            .Select(static (x, _) => x!);
    }

    extension<T>(IncrementalValuesProvider<T> source)
    {
        public IncrementalValuesProvider<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return source.Select((x, _) => selector(x));
        }

        public IncrementalValuesProvider<(T Left, TRight Right)> CombineWith<TRight>(IncrementalValueProvider<TRight> right)
        {
            return source.Combine(right);
        }

        public IncrementalValuesProvider<TResult> SelectMany<TResult>(Func<T, IEnumerable<TResult>> selector)
        {
            return source.SelectMany((x, _) => selector(x));
        }
    }
}
