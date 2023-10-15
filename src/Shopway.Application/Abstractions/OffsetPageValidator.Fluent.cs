using FluentValidation;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions.Common;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Application.Constants.PageConstants;

namespace Shopway.Application.Abstractions;

public abstract class OffsetPageValidator<TPageQuery, TResponse, TPage> : AbstractValidator<TPageQuery>
    where TResponse : IResponse
    where TPage : IOffsetPage
    where TPageQuery : IOffsetPageQuery<TResponse, TPage>
{
    public OffsetPageValidator()
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
    }
}