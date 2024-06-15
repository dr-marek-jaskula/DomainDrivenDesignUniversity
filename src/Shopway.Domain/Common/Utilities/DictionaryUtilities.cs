using System.Diagnostics.CodeAnalysis;

namespace Shopway.Domain.Common.Utilities;

public static class DictionaryUtilities
{
    public static Dictionary<TKey, TValue> AddIf<TKey, TValue>(this Dictionary<TKey, TValue> source, bool condition, TKey key, TValue value)
        where TKey : notnull
    {
        if (condition is false)
        {
            return source;
        }

        source.Add(key, value);
        return source;
    }

    public static TValue GetOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        where TValue : new()
    {
        if (source.TryGetValue(key, out var value) is false)
        {
            value = new TValue();
            source.Add(key, value);
        }

        return value;
    }

    public static TValue? Find<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key)
    {
        return source.TryGetValue(key, out var value) is false
            ? default
            : value;
    }

    public static bool TryGetAndRemove<TKey, TValue, TReturn>(this IDictionary<TKey, TValue> source, TKey key, [NotNullWhen(true)] out TReturn value)
    {
        if (source.TryGetValue(key, out var item) && item is not null)
        {
            source.Remove(key);
            value = (TReturn)(object)item;
            return true;
        }

        value = default!;
        return false;
    }

    public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> source, Func<TKey, TValue, bool> predicate)
    {
        source.Remove((k, v, p) => p!(k, v), predicate);
    }

    public static void Remove<TKey, TValue, TState>(this IDictionary<TKey, TValue> source, Func<TKey, TValue, TState?, bool> predicate, TState? state)
    {
        var found = false;
        var firstRemovedKey = default(TKey);
        List<KeyValuePair<TKey, TValue>>? pairsRemainder = null;

        foreach (var pair in source)
        {
            if (found)
            {
                pairsRemainder ??= [];
                pairsRemainder.Add(pair);
                continue;
            }

            if (predicate(pair.Key, pair.Value, state) is false)
            {
                continue;
            }

            if (found is false)
            {
                found = true;
                firstRemovedKey = pair.Key;
            }
        }

        if (found)
        {
            source.Remove(firstRemovedKey!);
            if (pairsRemainder == null)
            {
                return;
            }

            foreach (var (key, value) in pairsRemainder)
            {
                if (predicate(key, value, state))
                {
                    source.Remove(key);
                }
            }
        }
    }
}
