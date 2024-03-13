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
            .SetSortPriorities()
            .ToList()
            .ContainsDuplicates(x => x.SortPriority);
    }

    public static bool ContainsNullSortByProperty(this IList<SortByEntry> sortProperties)
    {
        return sortProperties
            .Any(x => x is null);
    }

    public static IList<SortByEntry> SetSortPriorities(this IEnumerable<SortByEntry> sortProperties)
    {
        return sortProperties
            .Select((property, index) =>
            {
                if (property.ParsedFromString)
                {
                    property.SortPriority = index;
                }

                return property;
            })
            .ToList();
    }
}