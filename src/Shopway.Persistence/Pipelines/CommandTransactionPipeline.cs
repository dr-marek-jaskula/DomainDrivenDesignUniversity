using MediatR;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Abstractions;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Persistence.Pipelines;

public sealed class CommandTransactionPipeline<TCommandRequest, TCommandResponse>
    : CommandTransactionPipelineBase<TCommandResponse>, IPipelineBehavior<TCommandRequest, TCommandResponse>
    where TCommandRequest : class, IRequest<TCommandResponse>, ICommand
    where TCommandResponse : class, IResult
{
    public CommandTransactionPipeline(IUnitOfWork<ShopwayDbContext> unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}