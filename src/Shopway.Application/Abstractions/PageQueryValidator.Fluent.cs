using FluentValidation;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using static Shopway.Application.Constants.PageConstants;
using static Shopway.Persistence.Utilities.OrderEntryUtilities;

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
            if (AllowedPageSizes.Contains(pageSize) is false)
            {
                context.AddFailure(PageSize, $"{PageSize} must be in: [{string.Join(",", AllowedPageSizes)}]");
            }
        });

        RuleFor(query => query.Order)
            .Must(ValidatePageOrderPropertyNames)
            .WithMessage($"SortProperties contains invalid property name.");

        RuleFor(query => query.Order)
            .Must(ValidatePageOrderPropertyPriorities)
            .WithMessage("SortProperties contains priority duplicates.");
    }

    private static bool ValidatePageOrderPropertyNames(TSortBy? pageOrder)
    {
        if (pageOrder is null)
        {
            return true;
        }

        if (pageOrder.SortProperties.AnyInvalidSortPropertyName(pageOrder.AllowedSortProperties))
        {
            return false;
        }

        return true;
    }

    private static bool ValidatePageOrderPropertyPriorities(TSortBy? pageOrder)
    {
        if (pageOrder is null)
        {
            return true;
        }

        if (pageOrder.SortProperties.DuplicatedSortPriority())
        {
            return false;
        }

        return true;
    }
}