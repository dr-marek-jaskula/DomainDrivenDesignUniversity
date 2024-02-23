using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing;

public static class MappingUtilities
{
    public static bool ContainsInvalidMappingProperty(this IList<string> mappingProperties, IReadOnlyCollection<string> allowedMappingProperties, out IReadOnlyCollection<string> invalidProperties)
    {
        invalidProperties = mappingProperties
            .Except(allowedMappingProperties)
            .ToList()
            .AsReadOnly();

        return invalidProperties.Count is not 0;
    }
}