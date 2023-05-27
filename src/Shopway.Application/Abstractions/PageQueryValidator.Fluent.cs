using FluentValidation;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Utilities;
using static Shopway.Application.Constants.PageConstants;
using static Shopway.Application.Constants.SortConstants;
using static Shopway.Domain.Utilities.SortByEntryUtilities;

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
                context.AddFailure(SortProperties, $"{SortProperties} contains invalid property name. Allowed property names: {string.Join(", ", order.AllowedSortProperties)}");
            }

            if (order.SortProperties.ContainsSortPriorityDuplicate())
            {
                context.AddFailure(SortProperties, $"{SortProperties} contains priority duplicates.");
            }
        });
    }
}