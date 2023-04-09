using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Application.Pipelines.CQRS.Batch;

public sealed class BatchCommandTransactionPipeline<TCommandRequest, TCommandResponse>
    : CommandTransactionPipelineBase<TCommandResponse>, IPipelineBehavior<TCommandRequest, TCommandResponse>
    where TCommandRequest : class, IRequest<TCommandResponse>, IBatchCommand<IBatchRequest>
    where TCommandResponse : class, IResult
{
    public BatchCommandTransactionPipeline(IUnitOfWork<ShopwayDbContext> unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}