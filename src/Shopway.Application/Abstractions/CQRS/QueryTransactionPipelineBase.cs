using MediatR;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using static Microsoft.EntityFrameworkCore.QueryTrackingBehavior;

namespace Shopway.Application.Abstractions.CQRS;

public abstract class QueryTransactionPipelineBase<TQueryResponse>
    where TQueryResponse : IResult
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

    protected async Task<TQueryResponse> BeginTransactionAsync(RequestHandlerDelegate<TQueryResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await UnitOfWork.BeginTransactionAsync(cancellationToken);
        return await next();
    }
}