using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface ICommand : IRequest<IResult>
{
}

public interface ICommand<out TResponse> : IRequest<IResult<TResponse>>
    where TResponse : IResponse
{
}
