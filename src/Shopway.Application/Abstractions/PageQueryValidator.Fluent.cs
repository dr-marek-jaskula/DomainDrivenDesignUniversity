using FluentValidation;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Application.Constants.Constants.Page;

namespace Shopway.Application.Abstractions;

public abstract class PageQueryValidator<TPageQuery, TPageResponse, TPage> : AbstractValidator<TPageQuery>
    where TPageResponse : IPageResponse
    where TPage : IPage
    where TPageQuery : IPageQuery<TPageResponse, TPage>
{
    protected PageQueryValidator()
    {
        RuleFor(query => query.Page.PageSize).Custom((pageSize, context) =>
        {
            if (AllowedPageSizes.NotContains(pageSize))
            {
                context.AddFailure(PageSize, $"{PageSize} must be in: [{AllowedPageSizes.Join(',')}]");
            }
        });
    }
}