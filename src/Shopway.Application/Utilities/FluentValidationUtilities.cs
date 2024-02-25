using FluentValidation;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using static Shopway.Application.Cache.ApplicationCache;
using static Shopway.Application.Constants.Constants.Filter;
using static Shopway.Application.Constants.Constants.Sort;
using static Shopway.Application.Constants.Constants.Mapping;
using static Shopway.Domain.Common.DataProcessing.FilterByEntryUtilities;
using static Shopway.Domain.Common.Utilities.SortByEntryUtilities;

namespace Shopway.Application.Utilities;

public static class FluentValidationUtilities
{
    public static void ValidateFilter<TFilter, TPageQuery>(TFilter filter, ValidationContext<TPageQuery> context)
        where TFilter : IFilter
    {
        if (filter is not IDynamicFilter dynamicFilter)
        {
            return;
        }

        if (dynamicFilter.FilterProperties.IsNullOrEmpty())
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} cannot be null or empty.");
            return;
        }

        var filterType = filter.GetType();

        AllowedFilterPropertiesCache.TryGetValue(filterType, out var allowedFilterPropertiesCache);

        if (dynamicFilter.FilterProperties!.ContainsInvalidFilterProperty(allowedFilterPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid property names: {string.Join(", ", invalidProperties)}. Allowed property names: {string.Join(", ", allowedFilterPropertiesCache!)}. {FilterProperties} are case sensitive.");
        }

        AllowedFilterOperationsCache.TryGetValue(filterType, out var allowedFilterOperations);

        if (dynamicFilter.FilterProperties.ContainsOnlyOperationsFrom(allowedFilterOperations!, out IReadOnlyCollection<string> invalidOperations))
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid operations: {string.Join(", ", invalidOperations)}. Allowed operations: {string.Join(", ", allowedFilterOperations!)}.");
        }
    }

    public static void ValidateSortBy<TSortBy, TPageQuery>(TSortBy sortBy, ValidationContext<TPageQuery> context)
        where TSortBy : ISortBy
    {
        if (sortBy is not IDynamicSortBy dynamicSortBy)
        {
            return;
        }

        if (dynamicSortBy.SortProperties.IsNullOrEmpty())
        {
            context.AddFailure(SortProperties, $"{SortProperties} cannot be null or empty.");
            return;
        }

        var sortByType = sortBy.GetType();

        AllowedSortPropertiesCache.TryGetValue(sortByType, out var allowedSortByPropertiesCache);

        if (dynamicSortBy.SortProperties.ContainsInvalidSortProperty(allowedSortByPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains invalid property names: {string.Join(", ", invalidProperties)}. Allowed property names: {string.Join(", ", allowedSortByPropertiesCache!)}. {SortProperties} are case sensitive.");
        }

        if (dynamicSortBy.SortProperties.ContainsSortPriorityDuplicate())
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains priority duplicates.");
        }
    }

    public static void ValidateMapping<TMapping, TPageQuery>(TMapping mapping, ValidationContext<TPageQuery> context)
        where TMapping : IMapping
    {
        if (mapping is null)
        {
            context.AddFailure(nameof(Constants.Constants.Mapping), $"Mapping must be provided.");
            return;
        }

        if (mapping is not IDynamicMapping dynamicMapping)
        {
            return;
        }

        if (dynamicMapping.MappingEntries.IsNullOrEmpty())
        {
            context.AddFailure(MappingProperties, $"{MappingProperties} cannot be null or empty.");
            return;
        }

        var mappingType = mapping.GetType();

        AllowedMappingPropertiesCache.TryGetValue(mappingType, out var allowedMappingPropertiesCache);

        if (dynamicMapping.MappingEntries.ContainsInvalidMappingProperty(allowedMappingPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
        {
            context.AddFailure(MappingProperties, $"{MappingProperties} contains invalid property names: {string.Join(", ", invalidProperties)}. Allowed property names: {string.Join(", ", allowedMappingPropertiesCache!)}. {MappingProperties} are case sensitive.");
        }
    }
}