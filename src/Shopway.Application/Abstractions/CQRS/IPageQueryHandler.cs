using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the page query handler interface
/// </summary>
/// <typeparam name="TQuery">The page query type</typeparam>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided page type</typeparam>
public interface IPageQueryHandler<TQuery, TResponse, TFilter, TSortBy, TPage> : IQueryHandler<TQuery, PageResponse<TResponse>>
    where TQuery : IPageQuery<TResponse, TFilter, TSortBy, TPage>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : IPage
{
}
