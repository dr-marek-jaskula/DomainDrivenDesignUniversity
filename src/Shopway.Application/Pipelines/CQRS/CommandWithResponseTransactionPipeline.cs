using MediatR;
using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class CommandWithResponseTransactionPipeline<TCommandRequest, TCommandResponse>
    : CommandTransactionPipelineBase<TCommandResponse>, IPipelineBehavior<TCommandRequest, TCommandResponse>
    where TCommandRequest : class, IRequest<TCommandResponse>, ICommand<IResponse>
    where TCommandResponse : class, IResult<IResponse>
{
    public CommandWithResponseTransactionPipeline(IUnitOfWork<ShopwayDbContext> unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}