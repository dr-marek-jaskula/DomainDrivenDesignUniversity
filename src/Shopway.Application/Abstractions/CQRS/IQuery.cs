using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IQuery<out TResponse> : IRequest<IResult<TResponse>>
    where TResponse : IResponse
{
}
