using FluentValidation;
using Shopway.Domain.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Application.Constants.PageConstants;

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
            .Must(ValidatePageOrder)
            .WithMessage("Invalid SortBy or ThenBy: Single SortBy can be selected and if so, single ThenBy can be chosen");
    }

    private static bool ValidatePageOrder(TSortBy? pageOrder)
    {
        if (pageOrder is null)
        {
            return true;
        }

        //var sortBy_NotNullCount = pageOrder
        //    .GetType()
        //    .GetProperties()
        //    .Where(p => p.Name.StartsWith(Then) is false)
        //    .Count(p => p.GetValue(pageOrder) is not null);

        //var thenBy_NotNullCount = pageOrder
        //    .GetType()
        //    .GetProperties()
        //    .Where(p => p.Name.StartsWith(Then) is true)
        //    .Count(p => p.GetValue(pageOrder) is not null);

        //if (sortBy_NotNullCount > 1 || thenBy_NotNullCount > 1)
        //{
        //    return false;
        //}

        //if (sortBy_NotNullCount is 0 && thenBy_NotNullCount is 1)
        //{
        //    return false;
        //}

        return true;
    }
}