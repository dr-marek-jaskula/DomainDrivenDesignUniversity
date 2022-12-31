using MediatR;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, IResult>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, IResult<TResponse>>
    where TCommand : ICommand<TResponse>
    where TResponse : IResponse
{
}
