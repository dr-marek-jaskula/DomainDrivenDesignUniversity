using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

public interface ICommand : IRequest<IResult>
{
}

public interface ICommand<out TResponse> : IRequest<IResult<TResponse>>
    where TResponse : IResponse
{
}
