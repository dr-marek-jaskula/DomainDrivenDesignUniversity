using System.Collections;

namespace Shopway.Domain.Common.Utilities;

public static class IEnumerableUtilities
{
    public static string Join<TValue>(this IEnumerable<TValue> items, char separator)
    {
        return string.Join(separator, items);
    }

    public static string Join<TValue>(this IEnumerable<TValue> items, string separator)
    {
        return string.Join(separator, items);
    }

    public static bool StructuralSequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        using var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();
        while (firstEnumerator.MoveNext())
        {
            if (secondEnumerator.MoveNext() is false 
                || StructuralComparisons.StructuralEqualityComparer.Equals(firstEnumerator.Current, secondEnumerator.Current) is false)
            {
                return false;
            }
        }

        return secondEnumerator.MoveNext() is false;
    }

    public static bool StartsWith<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        using var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();

        while (secondEnumerator.MoveNext())
        {
            if (firstEnumerator.MoveNext() is false || Equals(firstEnumerator.Current, secondEnumerator.Current) is false)
            {
                return false;
            }
        }

        return true;
    }

    public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
    {
        return source
            .Select((x, index) => comparer.Equals(item, x) ? index : -1)
            .FirstOr(x => x != -1, -1);
    }

    public static T FirstOr<T>(this IEnumerable<T> source, T alternate)
    {
        return source.DefaultIfEmpty(alternate).First();
    }

    public static T FirstOr<T>(this IEnumerable<T> source, Func<T, bool> predicate, T alternate)
    {
        return source.Where(predicate).FirstOr(alternate);
    }

    public static bool Any(this IEnumerable source)
    {
        foreach (var _ in source)
        {
            return true;
        }

        return false;
    }

    public static bool IsEmpty(this IEnumerable source)
    {
        return source.Any() is false;
    }

    public static async Task<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();
        await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            list.Add(element);
        }

        return list;
    }

    public static List<TSource> ToList<TSource>(this IEnumerable source)
    {
        return source.OfType<TSource>().ToList();
    }

    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T?, T?, bool> comparer)
        where T : class
    {
        return source.Distinct(new DynamicEqualityComparer<T>(comparer));
    }

    private sealed class DynamicEqualityComparer<T>(Func<T?, T?, bool> func) : IEqualityComparer<T>
        where T : class
    {
        private readonly Func<T?, T?, bool> _func = func;

        public bool Equals(T? x, T? y)
        {
            return _func(x, y);
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}