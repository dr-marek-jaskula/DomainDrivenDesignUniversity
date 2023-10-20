using Shopway.Application.Features;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the page query handler interface
/// </summary>
/// <typeparam name="TQuery">The page query type</typeparam>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQueryHandler<TQuery, TResponse, TFilter, TSortBy, TPage> : IQueryHandler<TQuery, CursorPageResponse<TResponse>>
    where TQuery : ICursorPageQuery<TResponse, TFilter, TSortBy, TPage>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : ICursorPage
{
}
