using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class ListQueryTransactionPipeline<TQueryRequest, TQueryResponse>
    : QueryTransactionPipelineBase<TQueryResponse>, IPipelineBehavior<TQueryRequest, TQueryResponse>
    where TQueryRequest : class, IRequest<TQueryResponse>, IListQuery<IResponse>
    where TQueryResponse : class, IResult<IList<IResponse>>
{
    public ListQueryTransactionPipeline(IUnitOfWork<ShopwayDbContext> unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TQueryResponse> Handle(TQueryRequest request, RequestHandlerDelegate<TQueryResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}