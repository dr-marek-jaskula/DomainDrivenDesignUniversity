using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing;

public static class MappingUtilities
{
    public static bool ContainsInvalidMappingProperty(this IList<MappingEntry> mappingEntries, IReadOnlyCollection<string> allowedMappingProperties, out IReadOnlyCollection<string> invalidProperties)
    {
        invalidProperties = mappingEntries
            .SelectMany(x => x.GetAllPropertyNames())
            .Except(allowedMappingProperties)
            .ToList()
            .AsReadOnly();

        return invalidProperties.Count is not 0;
    }

    public static bool ContainsNullMappingProperty(this IList<MappingEntry> mappingEntries)
    {
        return mappingEntries
            .Any(x => x is null || x.PropertyName is null && x.From is null);
    }
}