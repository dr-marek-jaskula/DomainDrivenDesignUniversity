using Shopway.Application.Features.Proxy;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Presentation.Authentication.GenericProxy;

public sealed record GenericProxyRequirementResource(string Entity, List<string> RequestedProperties)
{
    public static GenericProxyRequirementResource From(GenericProxyPageQuery query)
    {
        var allProperties = GetAllPropertiesWithHierarchy(query.Mapping?.MappingEntries, query.Filter?.FilterProperties, query.SortBy?.SortProperties);
        return new GenericProxyRequirementResource(query.Entity, allProperties);
    }

    private static List<string> GetAllPropertiesWithHierarchy(IList<MappingEntry>? mappingEntries, IList<FilterByEntry>? filterEntries, IList<SortByEntry>? sortEntries)
    {
        var filterProperties = filterEntries?
            .SelectMany(x => x.Predicates)
            .Select(x => x.PropertyName) ?? [];

        var sortProperties = sortEntries?
            .Select(x => x.PropertyName) ?? [];

        var mappingProperties = mappingEntries?
           .SelectMany(x => x.GetAllPropertyNamesWithHierarchy()) ?? [];

        return filterProperties
            .Concat(sortProperties)
            .Concat(mappingProperties)
            .Distinct()
            .ToList();
    }
}
