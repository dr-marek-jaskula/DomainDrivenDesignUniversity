using FluentValidation;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using static Shopway.Application.Features.Proxy.GenericValidators.GenericFluentValidationUtilities;

namespace Shopway.Application.Features.Proxy.GenericValidators;

/// <summary>
/// A generic offset page query validator, created to encapsulate common offset page query validation logic
/// </summary>
internal abstract class GenericOffsetPageQueryValidator<TPageQuery, TResponse, TFilter, TSortBy, TMapping, TPage> : PageQueryValidator<TPageQuery, OffsetPageResponse<TResponse>, TPage>
    where TResponse : IResponse
    where TFilter : IDynamicFilter
    where TSortBy : IDynamicSortBy
    where TMapping : IDynamicMapping
    where TPage : IOffsetPage
    where TPageQuery : IOffsetPageQuery<TResponse, TFilter, TSortBy, TMapping, TPage>
{
    protected GenericOffsetPageQueryValidator()
        : base()
    {
        RuleFor(query => query.Page.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.SortBy).Custom(ValidateSortBy);
        RuleFor(query => query.Filter).Custom(ValidateFilter);
        RuleFor(query => query.Mapping).Custom(ValidateMapping);
    }
}
