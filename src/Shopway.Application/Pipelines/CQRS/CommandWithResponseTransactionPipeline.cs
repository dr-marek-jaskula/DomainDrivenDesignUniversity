using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class CommandWithResponseTransactionPipeline<TCommandRequest, TCommandResponse>
    : CommandTransactionPipelineBase<TCommandResponse>, IPipelineBehavior<TCommandRequest, TCommandResponse>
    where TCommandRequest : class, IRequest<TCommandResponse>, ICommand<IResponse>
    where TCommandResponse : class, IResult<IResponse>
{
    public CommandWithResponseTransactionPipeline(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UniteOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}