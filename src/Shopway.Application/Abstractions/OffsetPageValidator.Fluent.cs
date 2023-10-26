using FluentValidation;
using Shopway.Application.Features;
using Shopway.Domain.Abstractions.Common;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Abstractions;

public abstract class OffsetPageQueryValidator<TPageQuery, TResponse, TPage> : PageQueryValidator<TPageQuery, OffsetPageResponse<TResponse>, TPage>
    where TResponse : IResponse
    where TPage : IOffsetPage
    where TPageQuery : IOffsetPageQuery<TResponse, TPage>
{
    public OffsetPageQueryValidator()
        : base()
    {
        RuleFor(query => query.Page.PageNumber)
            .GreaterThanOrEqualTo(1);
    }
}