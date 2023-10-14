using FluentValidation;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Application.Constants.PageConstants;
using static Shopway.Application.Constants.SortConstants;
using static Shopway.Application.Constants.FilterConstants;
using static Shopway.Domain.Utilities.SortByEntryUtilities;
using static Shopway.Domain.Utilities.FilterByEntryUtilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Application.Abstractions;

/// <summary>
/// A generic cursor page query validator, created to encapsulate common cursor page query validation logic
/// </summary>
internal abstract class CursorPageQueryValidator<TPageQuery, TResponse, TFilter, TSortBy, TPage> : AbstractValidator<TPageQuery>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : ICursorPage
    where TPageQuery : ICursorPageQuery<TResponse, TFilter, TSortBy, TPage>
{
    protected CursorPageQueryValidator() 
        : base()
    {
        RuleFor(query => query.Page.PageSize).Custom((pageSize, context) =>
        {
            if (AllowedPageSizes.NotContains(pageSize))
            {
                context.AddFailure(PageSize, $"{PageSize} must be in: [{string.Join(", ", AllowedPageSizes)}]");
            }
        });

        RuleFor(query => query.SortBy).Custom((sortBy, context) =>
        {
            if (sortBy is null)
            {
                return;
            }

            if (sortBy is not IDynamicSortBy dynamicSortBy)
            {
                return;
            }

            if (dynamicSortBy.SortProperties.ContainsInvalidSortProperty(dynamicSortBy.AllowedSortProperties, out IReadOnlyCollection<string> invalidProperties))
            {
                context.AddFailure(SortProperties, $"{SortProperties} contains invalid property names: {string.Join(", ", invalidProperties)}. Allowed property names: {string.Join(", ", dynamicSortBy.AllowedSortProperties)}. {SortProperties} are case sensitive.");
            }

            if (dynamicSortBy.SortProperties.ContainsSortPriorityDuplicate())
            {
                context.AddFailure(SortProperties, $"{SortProperties} contains priority duplicates.");
            }
        });

        RuleFor(query => query.Filter).Custom((filter, context) =>
        {
            if (filter is null)
            {
                return;
            }

            if (filter is not IDynamicFilter dynamicFilter)
            {
                return;
            }

            if (dynamicFilter.FilterProperties.ContainsInvalidFilterProperty(dynamicFilter.AllowedFilterProperties, out IReadOnlyCollection<string> invalidProperties))
            {
                context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid property names: {string.Join(", ", invalidProperties)}. Allowed property names: {string.Join(", ", dynamicFilter.AllowedFilterProperties)}. {FilterProperties} are case sensitive.");
            }

            if (dynamicFilter.FilterProperties.ContainsOnlyOperationsFrom(AllowedProductFilterOperations, out IReadOnlyCollection<string> invalidOperations))
            {
                context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid operations: {string.Join(", ", invalidOperations)}. Allowed operations: {string.Join(", ", AllowedProductFilterOperations)}.");
            }
        });
    }
}