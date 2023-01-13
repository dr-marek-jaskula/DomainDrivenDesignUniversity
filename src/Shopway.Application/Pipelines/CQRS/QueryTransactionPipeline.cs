using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class QueryTransactionPipeline<TQueryRequest, TQueryResponse>
    : QueryTransactionPipelineBase<TQueryResponse>, IPipelineBehavior<TQueryRequest, TQueryResponse>
    where TQueryRequest : class, IRequest<TQueryResponse>, IQuery<IResponse>
    where TQueryResponse : class, IResult<IResponse>
{
    public QueryTransactionPipeline(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TQueryResponse> Handle(TQueryRequest request, RequestHandlerDelegate<TQueryResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}