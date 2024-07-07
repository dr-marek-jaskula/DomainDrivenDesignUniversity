using FluentValidation;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Linq.Expressions;
using static Shopway.Application.Constants.Constants.Filter;
using static Shopway.Application.Constants.Constants.Mapping;
using static Shopway.Application.Constants.Constants.Sort;
using static Shopway.Domain.Common.DataProcessing.FilterByEntryUtilities;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Common.Utilities.SortByEntryUtilities;

namespace Shopway.Application.Features.Proxy.GenericValidators;

public static class GenericFluentValidationUtilities
{
    private const string All = nameof(All);
    private const string Any = nameof(Any);
    private const string Like = nameof(Like);
    private const string AllLike = $"{All}.{Like}";
    private const string AnyLike = $"{Any}.{Like}";
    private const string AllStartsWith = $"{All}.{nameof(string.StartsWith)}";
    private const string AnyStartsWith = $"{Any}.{nameof(string.StartsWith)}";
    private const string AllEndsWith = $"{All}.{nameof(string.EndsWith)}";
    private const string AnyEndsWith = $"{Any}.{nameof(string.EndsWith)}";
    private readonly static IReadOnlyCollection<string> _allowedOperations = AsList
    (
        nameof(string.Contains), 
        Like,
        nameof(string.StartsWith), 
        nameof(string.EndsWith),
        AllLike,
        AnyLike,
        AllStartsWith,
        AnyStartsWith,
        AllEndsWith,
        AnyEndsWith
    )
        .Concat(GetNamesOf<ExpressionType>())
        .Concat(GetNamesOf<ExpressionType>().Select(x => $"All.{x}"))
        .Concat(GetNamesOf<ExpressionType>().Select(x => $"Any.{x}"))
        .ToList()
        .AsReadOnly();

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

        if (filter.FilterProperties.ContainsOnlyOperationsFrom(_allowedOperations, out IReadOnlyCollection<string> invalidOperations))
        {
            context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid operations: {string.Join(", ", invalidOperations)}. Allowed operations: {string.Join(", ", _allowedOperations)}.");
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
