using FluentValidation;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;
using static Shopway.Application.Cache.ApplicationCache;
using static Shopway.Application.Constants.SortConstants;
using static Shopway.Application.Constants.FilterConstants;
using static Shopway.Domain.Utilities.SortByEntryUtilities;
using static Shopway.Domain.Utilities.FilterByEntryUtilities;

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

        var filterType = filter.GetType();

        AllowedFilterPropertiesCache.TryGetValue(filterType, out var allowedFilterPropertiesCache);

        if (dynamicFilter.FilterProperties.ContainsInvalidFilterProperty(allowedFilterPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
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
}