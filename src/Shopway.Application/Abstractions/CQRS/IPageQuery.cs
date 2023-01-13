using MediatR;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

/// <summary>
/// Represents the page query interface
/// </summary>
/// <typeparam name="TResponse">The page query response type</typeparam>
/// <typeparam name="TFilter">The provided filter type</typeparam>
/// <typeparam name="TSortBy">The provided order type</typeparam>
public interface IPageQuery<TResponse, TFilter, TSortBy> : IRequest<IResult<PageResponse<TResponse>>>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
{
    int PageNumber { get; }
    int PageSize { get; }
    TFilter? Filter { get; }
    TSortBy? Order { get; }
}
