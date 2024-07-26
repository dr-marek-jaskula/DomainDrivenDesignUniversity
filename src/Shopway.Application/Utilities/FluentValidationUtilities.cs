using FluentValidation;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Utilities;
using static Shopway.Application.Cache.ApplicationCache;
using static Shopway.Application.Constants.Constants.Filter;
using static Shopway.Application.Constants.Constants.Mapping;
using static Shopway.Application.Constants.Constants.Sort;
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

        if (dynamicFilter.FilterProperties.ContainsNullFilterProperty())
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains null or null predicate");
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

        if (dynamicSortBy.SortProperties.ContainsNullSortByProperty())
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains null");
            return;
        }

        if (dynamicSortBy.SortProperties.ContainsDuplicates(x => x.PropertyName))
        {
            context.AddFailure(SortProperties, $"{SortProperties} contains PropertyName duplicates.");
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

        if (dynamicMapping.MappingEntries.ContainsNullMappingProperty())
        {
            context.AddFailure(MappingProperties, $"Top level {MappingProperties} contains null or object with null PropertyName and null From");
            return;
        }

        if (dynamicMapping.MappingEntries.ContainsDuplicates(x => x.PropertyName is not null ? x.PropertyName : x.From))
        {
            context.AddFailure(MappingProperties, $"{MappingProperties} contains PropertyName duplicates.");
            return;
        }

        var mappingType = mapping.GetType();

        AllowedMappingPropertiesCache.TryGetValue(mappingType, out var allowedMappingPropertiesCache);

        if (dynamicMapping.MappingEntries.ContainsInvalidMappingProperty(allowedMappingPropertiesCache!, out IReadOnlyCollection<string> invalidProperties))
        {
            context.AddFailure(MappingProperties, $"{MappingProperties} contains invalid property names: {string.Join(", ", invalidProperties)}. Allowed property names: {string.Join(", ", allowedMappingPropertiesCache!)}. {MappingProperties} are case sensitive.");
        }
    }

    public static IRuleBuilderOptions<TInput, TOutput> MustSatisfyValueObjectValidation<TInput, TOutput>(this IRuleBuilder<TInput, TOutput> ruleBuilder, Func<TOutput, IList<Error>> validationMethod)
    {
        return (IRuleBuilderOptions<TInput, TOutput>)ruleBuilder.Custom((value, context) =>
        {
            IList<Error> errors = validationMethod(value);

            if (errors.NotNullOrEmpty())
            {
                foreach (var error in errors)
                {
                    context.AddFailure(error.Serialize());
                }
            }
        });
    }

    public static IRuleBuilderOptions<TInput, TInput> MustSatisfyValueObjectValidation<TInput>(this IRuleBuilder<TInput, TInput> ruleBuilder, Func<IList<Error>> validationMethod)
    {
        return (IRuleBuilderOptions<TInput, TInput>)ruleBuilder.Custom((value, context) =>
        {
            IList<Error> errors = validationMethod();

            if (errors.NotNullOrEmpty())
            {
                foreach (var error in errors)
                {
                    context.AddFailure(error.Serialize());
                }
            }
        });
    }

    public static IRuleBuilderOptions<TInput, string> MustBeAnEnum<TInput, TEnum>(this IRuleBuilder<TInput, string> ruleBuilder)
        where TEnum : struct, Enum
    {
        return (IRuleBuilderOptions<TInput, string>)ruleBuilder.Custom((value, context) =>
        {
            if (Enum.TryParse<TEnum>(value, out var _) is false)
            {
                context.AddFailure(Error.InvalidArgument($"{value} is not a valid {typeof(TEnum).Name}").Serialize());
            }
        });
    }

    public static IRuleBuilderOptions<TInput, TOutput> MustSatisfy<TInput, TOutput>(this IRuleBuilder<TInput, TOutput> ruleBuilder, Func<TOutput, IList<Error>> validationMethod)
    {
        return (IRuleBuilderOptions<TInput, TOutput>)ruleBuilder.Custom((value, context) =>
        {
            IList<Error> errors = validationMethod(value);

            if (errors.NotNullOrEmpty())
            {
                foreach (var error in errors)
                {
                    context.AddFailure(error.Serialize());
                }
            }
        });
    }
}
