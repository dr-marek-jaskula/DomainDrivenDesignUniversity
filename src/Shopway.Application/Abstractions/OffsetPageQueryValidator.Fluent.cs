﻿using FluentValidation;
using Shopway.Application.Features;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using static Shopway.Application.Utilities.FluentValidationUtilities;

namespace Shopway.Application.Abstractions;

public abstract class OffsetPageQueryValidator<TPageQuery, TResponse, TPage> : PageQueryValidator<TPageQuery, OffsetPageResponse<TResponse>, TPage>
    where TResponse : IResponse
    where TPage : IOffsetPage
    where TPageQuery : IOffsetPageQuery<TResponse, TPage>
{
    protected OffsetPageQueryValidator()
        : base()
    {
        RuleFor(query => query.Page.PageNumber)
            .GreaterThanOrEqualTo(1);
    }
}

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
        RuleFor(query => query.SortBy).Custom(ValidateSortBy!);
        RuleFor(query => query.Filter).Custom(ValidateFilter!);
    }
}