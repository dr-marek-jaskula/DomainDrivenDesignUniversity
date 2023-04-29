using Shopway.Domain.Helpers;
using System.Collections.ObjectModel;

namespace Shopway.Persistence.Utilities;

public static class OrderEntryUtilities
{
    public static bool AnyInvalidSortPropertyName(this IList<OrderEntry> sortProperties, ReadOnlyCollection<string> allowedSortProperties)
    {
        return sortProperties
            .Select(x => x.PropertyName)
            .Except(allowedSortProperties)
            .Any();
    }

    public static bool DuplicatedSortPriority(this IList<OrderEntry> sortProperties)
    {
        return sortProperties
            .GroupBy(x => x.SortPriority)
            .Any(g => g.Count() > 1);
    }
}