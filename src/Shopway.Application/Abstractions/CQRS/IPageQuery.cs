using MediatR;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

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
