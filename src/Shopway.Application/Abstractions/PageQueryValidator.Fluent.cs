using FluentValidation;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Utilities;
using static Shopway.Application.Constants.PageConstants;
using static Shopway.Application.Constants.SortConstants;
using static Shopway.Application.Constants.FilterConstants;
using static Shopway.Domain.Utilities.SortByEntryUtilities;
using static Shopway.Domain.Utilities.FilterByEntryUtilities;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Application.Abstractions;

/// <summary>
/// A generic page query validator, created to encapsulate common page query validation logic
/// </summary>
internal abstract class PageQueryValidator<TPageQuery, TResponse, TFilter, TSortBy, TPage> : AbstractValidator<TPageQuery>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : IPage
    where TPageQuery : IPageQuery<TResponse, TFilter, TSortBy, TPage>
{
    public PageQueryValidator()
    {
        RuleFor(query => query.Page.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.Page.PageSize).Custom((pageSize, context) =>
        {
            if (AllowedPageSizes.NotContains(pageSize))
            {
                context.AddFailure(PageSize, $"{PageSize} must be in: [{string.Join(", ", AllowedPageSizes)}]");
            }
        });

        RuleFor(query => query.Order).Custom((order, context) =>
        {
            if (order is null)
            {
                return;
            }

            if (order.SortProperties.ContainsInvalidSortProperty(order.AllowedSortProperties))
            {
                context.AddFailure(SortProperties, $"{SortProperties} contains invalid property name. Allowed property names: {string.Join(", ", order.AllowedSortProperties)}. {SortProperties} are case sensitive.");
            }

            if (order.SortProperties.ContainsSortPriorityDuplicate())
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

            if (filter is not IExpressionFilter expressionFilter)
            {
                return;
            }

            if (expressionFilter.FilterProperties.ContainsInvalidFilterProperty(expressionFilter.AllowedFilterProperties))
            {
                context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid property name. Allowed property names: {string.Join(", ", expressionFilter.AllowedFilterProperties)}. {FilterProperties} are case sensitive.");
            }

            IReadOnlyCollection<string> invalidOperations;
            if (expressionFilter.FilterProperties.ContainsOnlyOperationsFrom(AllowedProductFilterOperations, out invalidOperations))
            {
                context.AddFailure(FilterProperties, $"{FilterProperties} contains invalid operations: {string.Join(", ", invalidOperations)}. Allowed operations: {string.Join(", ", AllowedProductFilterOperations)}.");
            }
        });
    }
}