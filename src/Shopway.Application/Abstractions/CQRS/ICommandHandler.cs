using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions.CQRS;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, IResult>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, IResult<TResponse>>
    where TCommand : ICommand<TResponse>
    where TResponse : IResponse
{
}
