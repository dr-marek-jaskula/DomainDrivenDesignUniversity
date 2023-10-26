using FluentValidation;
using Shopway.Domain.Abstractions.Common;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Application.Utilities.FluentValidationUtilities;

namespace Shopway.Application.Abstractions;

/// <summary>
/// A generic offset page query validator, created to encapsulate common offset page query validation logic
/// </summary>
internal abstract class OffsetPageQueryValidator<TPageQuery, TResponse, TFilter, TSortBy, TPage> : OffsetPageQueryValidator<TPageQuery, TResponse, TPage>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : IOffsetPage
    where TPageQuery : IOffsetPageQuery<TResponse, TFilter, TSortBy, TPage>
{
    protected OffsetPageQueryValidator() 
        : base()
    {
        RuleFor(query => query.SortBy).Custom(ValidateSortBy);
        RuleFor(query => query.Filter).Custom(ValidateFilter);
    }
}