using Shopway.Domain.Common;

namespace Shopway.Domain.Utilities;

public static class SortByEntryUtilities
{
    public static bool ContainsInvalidSortProperty(this IList<SortByEntry> sortProperties, IReadOnlyCollection<string> allowedSortProperties, out IReadOnlyCollection<string> invalidProperties)
    {
        invalidProperties = sortProperties
            .Select(x => x.PropertyName)
            .Except(allowedSortProperties)
            .ToList()
            .AsReadOnly();

        return invalidProperties.Any();
    }

    public static bool ContainsSortPriorityDuplicate(this IList<SortByEntry> sortProperties)
    {
        return sortProperties
            .ContainsDuplicates(x => x.SortPriority);
    }
}