namespace Shopway.Domain.Common.DataProcessing;

public static class FilterByEntryUtilities
{
    public static bool ContainsInvalidFilterProperty(this IList<FilterByEntry> filterProperties, IReadOnlyCollection<string> allowedFilterProperties, out IReadOnlyCollection<string> invalidProperties)
    {
        invalidProperties = filterProperties
            .SelectMany(x => x.Predicates)
            .Select(x => x.PropertyName)
            .Except(allowedFilterProperties)
            .ToList()
            .AsReadOnly();

        return invalidProperties.Any();
    }

    public static bool ContainsOnlyOperationsFrom(this IList<FilterByEntry> filterProperties, IReadOnlyCollection<string> allowedOperations, out IReadOnlyCollection<string> invalidOperations)
    {
        invalidOperations = filterProperties
            .SelectMany(x => x.Predicates)
            .Select(x => x.Operation)
            .Except(allowedOperations)
            .ToList()
            .AsReadOnly();

        return invalidOperations.Any();
    }
}