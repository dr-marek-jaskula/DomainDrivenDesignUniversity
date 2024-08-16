using FluentValidation;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using static Shopway.Application.Features.Proxy.GenericValidators.GenericFluentValidationUtilities;

namespace Shopway.Application.Features.Proxy.GenericValidators;

/// <summary>
/// A generic cursor page query validator, created to encapsulate common cursor page query validation logic
/// </summary>
internal abstract class GenericCursorPageQueryValidator<TPageQuery, TResponse, TFilter, TSortBy, TMapping, TPage> : PageQueryValidator<TPageQuery, CursorPageResponse<TResponse>, TPage>
    where TResponse : IResponse, IHasCursor
    where TFilter : IDynamicFilter
    where TSortBy : IDynamicSortBy
    where TMapping : IDynamicMapping
    where TPage : ICursorPage
    where TPageQuery : ICursorPageQuery<TResponse, TFilter, TSortBy, TMapping, TPage>
{
    protected GenericCursorPageQueryValidator()
        : base()
    {
        RuleFor(query => query.SortBy).Custom(ValidateSortBy);
        RuleFor(query => query.Filter).Custom(ValidateFilter);
        RuleFor(query => query.Mapping).Custom(ValidateMapping);
    }
}
