using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
