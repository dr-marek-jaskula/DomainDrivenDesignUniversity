using FluentValidation;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using static Shopway.Application.Constants.Constants.Filter;
using static Shopway.Application.Constants.Constants.Mapping;
using static Shopway.Application.Constants.Constants.Sort;
using static Shopway.Domain.Common.DataProcessing.FilterByEntryUtilities;
using static Shopway.Domain.Common.Utilities.SortByEntryUtilities;

namespace Shopway.Application.Features.Proxy.GenericValidators;

public static class GenericFluentValidationUtilities
{
    public static void ValidateFilter<TFilter, TPageQuery>(TFilter? filter, ValidationContext<TPageQuery> context)
        where TFilter : IDynamicFilter
    {
        if (filter is null)
        {
            context.AddFailure(nameof(Constants.Constants.Filter), $"Filtering must be provided.");
            return;
        }

        if (filter.FilterProperties.IsNullOrEmpty())
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} cannot be null or empty.");
            return;
        }

        if (filter.FilterProperties.ContainsNullFilterProperty())
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains null or null predicate");
            return;
        }
    }

    public static void ValidateSortBy<TSortBy, TPageQuery>(TSortBy? sortBy, ValidationContext<TPageQuery> context)
        where TSortBy : IDynamicSortBy
    {
        if (sortBy is null)
        {
            context.AddFailure(nameof(Constants.Constants.Sort), $"Sorting must be provided.");
            return;
        }

        if (sortBy.SortProperties.IsNullOrEmpty())
        {
            context.AddFailure(SortProperties, $"{SortProperties} cannot be null or empty.");
            return;
        }

        if (sortBy.SortProperties.ContainsNullSortByProperty())
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains null");
            return;
        }

        if (sortBy.SortProperties.ContainsDuplicates(x => x.PropertyName))
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains PropertyName duplicates.");
            return;
        }

        if (sortBy.SortProperties.ContainsSortPriorityDuplicate())
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains priority duplicates.");
        }
    }

    public static void ValidateMapping<TMapping, TPageQuery>(TMapping? mapping, ValidationContext<TPageQuery> context)
        where TMapping : IDynamicMapping
    {
        if (mapping is null)
        {
            context.AddFailure(nameof(Constants.Constants.Mapping), $"Mapping must be provided.");
            return;
        }

        if (mapping.MappingEntries.IsNullOrEmpty())
        {
            context.AddFailure(MappingProperties, $"{MappingProperties} cannot be null or empty.");
            return;
        }

        if (mapping.MappingEntries.ContainsNullMappingProperty())
        {
            context.AddFailure(MappingProperties, $"Top level {MappingProperties} contains null or object with null PropertyName and null From");
            return;
        }

        if (mapping.MappingEntries.ContainsDuplicates(x => x.PropertyName is not null ? x.PropertyName : x.From))
        {
            context.AddFailure(MappingProperties, $"{MappingProperties} contains PropertyName duplicates.");
            return;
        }
    }
}
