using MediatR;
using Shopway.Persistence.Framework;
using Shopway.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Abstractions;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Persistence.Pipelines;

public sealed class CommandTransactionPipeline<TCommandRequest, TCommandResponse>(IUnitOfWork<ShopwayDbContext> unitOfWork)
    : CommandTransactionPipelineBase<TCommandResponse>(unitOfWork), 
      IPipelineBehavior<TCommandRequest, TCommandResponse>
        where TCommandRequest : class, IRequest<TCommandResponse>, ICommand
        where TCommandResponse : class, IResult
{
    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}