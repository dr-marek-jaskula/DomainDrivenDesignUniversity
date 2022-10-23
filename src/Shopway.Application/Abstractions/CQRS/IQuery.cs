using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
