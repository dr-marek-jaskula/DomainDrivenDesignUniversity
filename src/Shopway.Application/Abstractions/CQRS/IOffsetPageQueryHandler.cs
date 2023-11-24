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
/// <typeparam name="TPage">The provided offset page type</typeparam>
public interface IOffsetPageQueryHandler<TQuery, TResponse, TFilter, TSortBy, TPage> : IOffsetPageQueryHandler<TQuery, TResponse, TPage>
    where TQuery : IOffsetPageQuery<TResponse, TFilter, TSortBy, TPage>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
    where TPage : IOffsetPage
{
}

/// <summary>
/// Represents the page query handler interface
/// </summary>
/// <typeparam name="TQuery">The page query type</typeparam>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TPage">The provided offset page type</typeparam>
public interface IOffsetPageQueryHandler<TQuery, TResponse, TPage> : IQueryHandler<TQuery, OffsetPageResponse<TResponse>>
    where TQuery : IOffsetPageQuery<TResponse, TPage>
    where TResponse : IResponse
    where TPage : IOffsetPage
{
}
