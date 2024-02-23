using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Domain.Common.Utilities;

public static class SortByEntryUtilities
{
    public static bool ContainsInvalidSortProperty(this IList<SortByEntry> sortProperties, IReadOnlyCollection<string> allowedSortProperties, out IReadOnlyCollection<string> invalidProperties)
    {
        invalidProperties = sortProperties
            .Select(x => x.PropertyName)
            .Except(allowedSortProperties)
            .ToList()
            .AsReadOnly();

        return invalidProperties.Count is not 0;
    }

    public static bool ContainsSortPriorityDuplicate(this IList<SortByEntry> sortProperties)
    {
        return sortProperties
            .ContainsDuplicates(x => x.SortPriority);
    }
}