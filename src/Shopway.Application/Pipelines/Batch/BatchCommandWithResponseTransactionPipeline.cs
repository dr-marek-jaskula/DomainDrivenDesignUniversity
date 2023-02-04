using MediatR;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Application.Abstractions.Batch;
using Microsoft.EntityFrameworkCore;

namespace Shopway.Application.Pipelines.Batch;

public sealed class BatchCommandWithResponseTransactionPipeline<TCommandRequest, TCommandResponse>
    : CommandTransactionPipelineBase<TCommandResponse>, IPipelineBehavior<TCommandRequest, TCommandResponse>
    where TCommandRequest : class, IRequest<TCommandResponse>, IBatchCommand<IBatchRequest, IBatchResponse>
    where TCommandResponse : class, IResult<IBatchResponse>
{
    public BatchCommandWithResponseTransactionPipeline(IUnitOfWork<ShopwayDbContext> unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TCommandResponse> Handle(TCommandRequest request, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UniteOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}