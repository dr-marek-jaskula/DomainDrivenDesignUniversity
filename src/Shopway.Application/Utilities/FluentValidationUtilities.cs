using FluentValidation;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;
using static Shopway.Application.Cache.ApplicationCache;
using static Shopway.Application.Constants.Constants.Sort;
using static Shopway.Domain.Utilities.SortByEntryUtilities;
using static Shopway.Application.Constants.Constants.Filter;
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
        
        if (dynamicFilter.FilterProperties.IsNullOrEmpty())
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} cannot be null or empty.");
            return;
        }

        var filterType = filter.GetType();

        AllowedFilterPropertiesCache.TryGetValue(filterType, out var allowedFilterPropertiesCache);

        if (dynamicFilter.FilterProperties!.ContainsInvalidFilterProperty(allowedFilterPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid property names: {invalidProperties.Join(", ")}. Allowed property names: {allowedFilterPropertiesCache!.Join(", ")}. {FilterProperties} are case sensitive.");
        }

        AllowedFilterOperationsCache.TryGetValue(filterType, out var allowedFilterOperations);

        if (dynamicFilter.FilterProperties.ContainsOnlyOperationsFrom(allowedFilterOperations!, out IReadOnlyCollection<string> invalidOperations))
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid operations: {invalidOperations.Join(", ")}. Allowed operations: {allowedFilterOperations!.Join(", ")}.");
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
            context.AddFailure(FilterProperties, $"{SortProperties} cannot be null or empty.");
            return;
        }

        var sortByType = sortBy.GetType();

        AllowedSortPropertiesCache.TryGetValue(sortByType, out var allowedSortByPropertiesCache);

        if (dynamicSortBy.SortProperties.ContainsInvalidSortProperty(allowedSortByPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains invalid property names: {invalidProperties.Join(", ")}. Allowed property names: {allowedSortByPropertiesCache!.Join(", ")}. {SortProperties} are case sensitive.");
        }

        if (dynamicSortBy.SortProperties.ContainsSortPriorityDuplicate())
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains priority duplicates.");
        }
    }
}