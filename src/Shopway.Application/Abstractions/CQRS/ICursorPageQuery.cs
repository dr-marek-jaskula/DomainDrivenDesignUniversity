using Shopway.Application.Features;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the cursor page query interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TMapping">The provided mapping type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQuery<TResponse, TFilter, TSortBy, TMapping, TPage> : ICursorPageQuery<TResponse, TFilter, TSortBy, TPage>
    where TResponse : IResponse, IHasCursor
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TMapping : IMapping
    where TPage : ICursorPage
{
    TMapping? Mapping { get; }
}

/// <summary>
/// Represents the cursor page query interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQuery<TResponse, TFilter, TSortBy, TPage> : ICursorPageQuery<TResponse, TPage>
    where TResponse : IResponse, IHasCursor
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : ICursorPage
{
    TFilter? Filter { get; }
    TSortBy? SortBy { get; }
}

/// <summary>
/// Represents the cursor page interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TPage">The provided offset page type</typeparam>
public interface ICursorPageQuery<TResponse, TPage> : IPageQuery<CursorPageResponse<TResponse>, TPage>
    where TResponse : IResponse, IHasCursor
    where TPage : ICursorPage
{
}
