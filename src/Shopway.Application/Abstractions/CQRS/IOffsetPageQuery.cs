using Shopway.Application.Features;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the offset page query interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided offset page type</typeparam>
public interface IOffsetPageQuery<TResponse, TFilter, TSortBy, TPage> : IOffsetPageQuery<TResponse, TPage>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : IOffsetPage
{
    TFilter? Filter { get; }
    TSortBy? SortBy { get; }
}

/// <summary>
/// Represents the offset page interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TPage">The provided offset page type</typeparam>
public interface IOffsetPageQuery<TResponse, TPage> : IPageQuery<OffsetPageResponse<TResponse>, TPage>
    where TResponse : IResponse
    where TPage : IOffsetPage
{
}