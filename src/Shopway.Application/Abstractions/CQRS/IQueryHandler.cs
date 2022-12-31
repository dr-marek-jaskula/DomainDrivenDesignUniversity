using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, IResult<TResponse>>
    where TQuery : IQuery<TResponse>
    where TResponse : IResponse
{
}
