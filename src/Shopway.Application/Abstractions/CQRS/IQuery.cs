using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

public interface IQuery<out TResponse> : IRequest<IResult<TResponse>>
    where TResponse : IResponse
{
}
