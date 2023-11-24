namespace Shopway.Domain.Common.Utilities;

public static class ListUtilities
{
    public static List<TValue> AsList<TValue>(params TValue[] items)
    {
        return new List<TValue>(items);
    }

    public static List<TValue> EmptyList<TValue>()
    {
        return [];
    }

    public static bool NotNullOrEmpty<TValue>(this IList<TValue> list)
    {
        return list is not null && list.Any();
    }

    public static bool IsNullOrEmpty<TValue>(this IList<TValue> list)
    {
        return list.NotNullOrEmpty() is false;
    }

    public static bool NotContains<TValue>(this IList<TValue> list, TValue value)
    {
        return list.Contains(value) is false;
    }

    public static bool ContainsDuplicates<TValue, TType>(this IList<TValue> list, Func<TValue, TType> expression)
    {
        return list
            .GroupBy(expression)
            .Any(g => g.Count() > 1);
    }
}