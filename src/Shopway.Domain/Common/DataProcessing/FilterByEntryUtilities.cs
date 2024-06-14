namespace Shopway.Domain.Common.DataProcessing;

public static class FilterByEntryUtilities
{
    public static bool ContainsInvalidFilterProperty(this IList<FilterByEntry> filterEntries, IReadOnlyCollection<string> allowedFilterProperties, out IReadOnlyCollection<string> invalidProperties)
    {
        invalidProperties = filterEntries
            .SelectMany(x => x.Predicates)
            .Select(x => x.PropertyName)
            .Except(allowedFilterProperties)
            .ToList()
            .AsReadOnly();

        return invalidProperties.Count is not 0;
    }

    public static bool ContainsOnlyOperationsFrom(this IList<FilterByEntry> filterEntries, IReadOnlyCollection<string> allowedOperations, out IReadOnlyCollection<string> invalidOperations)
    {
        invalidOperations = filterEntries
            .SelectMany(x => x.Predicates)
            .Select(x => x.Operation)
            .Except(allowedOperations)
            .ToList()
            .AsReadOnly();

        return invalidOperations.Count is not 0;
    }

    public static bool ContainsNullFilterProperty(this IList<FilterByEntry> filterProperties)
    {
        return filterProperties
            .Any(x => x is null || x.Predicates.Any(y => y is null));
    }
}
