using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Presentation.Authentication.GenericProxy;

public sealed record GenericProxyRequirementResource(Type EntityType, string[] RequestedProperties)
{
    public static GenericProxyRequirementResource From(GenericProxyPageQuery query, IQuery<PageResponse<DataTransferObjectResponse>> mappedPageQuery)
    {
        var allProperties = GetAllPropertiesWithHierarchy(query.Mapping?.MappingEntries, query.Filter?.FilterProperties, query.SortBy?.SortProperties);
        Type type = mappedPageQuery.GetType().GetGenericArguments().First(x => x.Implements<IEntity>());
        return new GenericProxyRequirementResource(type, allProperties);
    }

    private static string[] GetAllPropertiesWithHierarchy(IList<MappingEntry>? mappingEntries, IList<FilterByEntry>? filterEntries, IList<SortByEntry>? sortEntries)
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
            .ToArray();
    }
}
