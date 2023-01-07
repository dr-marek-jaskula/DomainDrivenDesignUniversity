using MediatR;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IPageQueryHandler<TQuery, TResponse, TFilter, TSortBy> : IRequestHandler<TQuery, IResult<PageResponse<TResponse>>>
    where TQuery : IPageQuery<TResponse, TFilter, TSortBy>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
{
}
