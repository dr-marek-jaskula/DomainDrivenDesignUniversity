using MediatR;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;
using static Microsoft.EntityFrameworkCore.QueryTrackingBehavior;

namespace Shopway.Persistence.Abstractions;

public abstract class QueryTransactionPipelineBase<TQueryResponse>
    where TQueryResponse : IResult
{
    protected readonly IUnitOfWork<ShopwayDbContext> UnitOfWork;

    protected QueryTransactionPipelineBase(IUnitOfWork<ShopwayDbContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
        UnitOfWork
            .Context
            .ChangeTracker
            .QueryTrackingBehavior = NoTracking;
    }

    protected async Task<TQueryResponse> BeginTransactionAsync(RequestHandlerDelegate<TQueryResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await UnitOfWork.BeginTransactionAsync(cancellationToken);
        return await next();
    }
}