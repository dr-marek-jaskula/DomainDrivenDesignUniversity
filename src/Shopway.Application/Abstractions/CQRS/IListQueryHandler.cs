using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IListQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, IResult<IList<TResponse>>>
    where TQuery : IListQuery<TResponse>
    where TResponse : IResponse
{
}