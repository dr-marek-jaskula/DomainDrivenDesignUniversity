using Shopway.Application.Features;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the page query handler interface
/// </summary>
/// <typeparam name="TQuery">The page query type</typeparam>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TMapping">The provided mapping type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQueryHandler<TQuery, TResponse, TFilter, TSortBy, TMapping, TPage> : ICursorPageQueryHandler<TQuery, TResponse, TFilter, TSortBy, TPage>
    where TQuery : ICursorPageQuery<TResponse, TFilter, TSortBy, TMapping, TPage>
    where TResponse : IResponse, IHasCursor
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TMapping : IMapping
    where TPage : ICursorPage
{
}

/// <summary>
/// Represents the page query handler interface
/// </summary>
/// <typeparam name="TQuery">The page query type</typeparam>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQueryHandler<TQuery, TResponse, TFilter, TSortBy, TPage> : ICursorPageQueryHandler<TQuery, TResponse, TPage>
    where TQuery : ICursorPageQuery<TResponse, TFilter, TSortBy, TPage>
    where TResponse : IResponse, IHasCursor
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : ICursorPage
{
}

/// <summary>
/// Represents the page query handler interface
/// </summary>
/// <typeparam name="TQuery">The page query type</typeparam>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TPage">The provided cursor page type</typeparam>
public interface ICursorPageQueryHandler<TQuery, TResponse, TPage> : IQueryHandler<TQuery, CursorPageResponse<TResponse>>
    where TQuery : ICursorPageQuery<TResponse, TPage>
    where TResponse : IResponse, IHasCursor
    where TPage : ICursorPage
{
}
