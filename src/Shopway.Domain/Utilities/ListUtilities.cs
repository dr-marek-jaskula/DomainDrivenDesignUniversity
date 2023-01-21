using System.Collections.ObjectModel;

namespace Shopway.Domain.Utilities;

public static class ListUtilities
{
    public static List<TValue> AsList<TValue>(params TValue[] items)
    {
        return new List<TValue>(items);
    }

    public static List<TValue> Empty<TValue>()
    {
        return new List<TValue>();
    }

    public static IList<TValue> AsReadOnlyList<TValue>(params TValue[] items)
    {
        return new ReadOnlyCollection<TValue>(items);
    }

    public static bool IsNotNullOrEmpty<TValue>(this IList<TValue> list)
    {
        return list is not null && list.Any();
    }

    public static bool IsNullOrEmpty<TValue>(this IList<TValue> list)
    {
        return list.IsNotNullOrEmpty() is false;
    }
}