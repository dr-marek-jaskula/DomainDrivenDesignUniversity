using Shopway.Application.Features;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the page query interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQuery<TResponse, TFilter, TSortBy, TPage> : IQuery<CursorPageResponse<TResponse>>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : ICursorPage
{
    TPage Page { get; }
    TFilter? Filter { get; }
    TSortBy? SortBy { get; }
}
