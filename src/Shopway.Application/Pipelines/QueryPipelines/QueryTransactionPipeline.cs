using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.Pipelines.QueryPipelines;

public sealed class QueryTransactionPipeline<TQuery, TQueryResult>
    : QueryTransactionPipelineBase<TQueryResult>, IPipelineBehavior<TQuery, TQueryResult>
    where TQuery : IQuery<IResponse>
    where TQueryResult : IResult<IResponse>
{
    public QueryTransactionPipeline(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TQueryResult> Handle(TQuery request, RequestHandlerDelegate<TQueryResult> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}