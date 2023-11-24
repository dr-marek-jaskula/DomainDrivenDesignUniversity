using System.Collections.ObjectModel;

namespace Shopway.Domain.Common.Utilities;

public static class CollectionUtilities
{
    public static IReadOnlyCollection<TValue> AsReadOnlyCollection<TValue>(params TValue[] items)
    {
        return new ReadOnlyCollection<TValue>(items);
    }
}