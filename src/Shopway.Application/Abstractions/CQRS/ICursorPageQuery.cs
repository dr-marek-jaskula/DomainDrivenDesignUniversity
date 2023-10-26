using Shopway.Application.Features;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the cursor page query interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQuery<TResponse, TFilter, TSortBy, TPage> : ICursorPageQuery<TResponse, TPage>
    where TResponse : IResponse
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
    where TResponse : IResponse
    where TPage : ICursorPage
{
}