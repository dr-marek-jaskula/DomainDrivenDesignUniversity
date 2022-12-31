using MediatR;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using static Microsoft.EntityFrameworkCore.QueryTrackingBehavior;

namespace Shopway.Application.Pipelines.QueryPipelines;

public abstract class QueryTransactionPipelineBase<TQueryResult>
    where TQueryResult : IResult
{
    protected readonly IUnitOfWork UnitOfWork;

    protected QueryTransactionPipelineBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        UnitOfWork
            .Context
            .ChangeTracker
            .QueryTrackingBehavior = NoTracking;
    }

    protected async Task<TQueryResult> BeginTransactionAsync(RequestHandlerDelegate<TQueryResult> next, CancellationToken cancellationToken)
    {
        using var transaction = await UnitOfWork.BeginTransactionAsync(cancellationToken);
        return await next();
    }
}