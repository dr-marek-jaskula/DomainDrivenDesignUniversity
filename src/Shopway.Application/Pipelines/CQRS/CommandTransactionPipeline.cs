using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class CommandTransactionPipeline<TCommandRequest, TCommandResponse>
    : CommandTransactionPipelineBase<TCommandResponse>, IPipelineBehavior<TCommandRequest, TCommandResponse>
    where TCommandRequest : class, IRequest<TCommandResponse>, ICommand
    where TCommandResponse : class, IResult
{
    public CommandTransactionPipeline(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UniteOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}