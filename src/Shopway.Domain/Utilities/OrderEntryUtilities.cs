using Shopway.Domain.Common;
using System.Collections.ObjectModel;

namespace Shopway.Persistence.Utilities;

public static class OrderEntryUtilities
{
    public static bool AnyInvalidSortPropertyName(this IList<SortByEntry> sortProperties, ReadOnlyCollection<string> allowedSortProperties)
    {
        return sortProperties
            .Select(x => x.PropertyName)
            .Except(allowedSortProperties)
            .Any();
    }

    public static bool DuplicatedSortPriority(this IList<SortByEntry> sortProperties)
    {
        return sortProperties
            .GroupBy(x => x.SortPriority)
            .Any(g => g.Count() > 1);
    }
}