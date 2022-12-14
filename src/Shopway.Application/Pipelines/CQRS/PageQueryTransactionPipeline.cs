using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class PageQueryTransactionPipeline<TQueryRequest, TQueryResponse>
    : QueryTransactionPipelineBase<TQueryResponse>, IPipelineBehavior<TQueryRequest, TQueryResponse>
    where TQueryRequest : class, IRequest<TQueryResponse>, IPageQuery<IResponse, IFilter, ISortBy>
    where TQueryResponse : class, IResult<PageResponse<IResponse>>
{
    public PageQueryTransactionPipeline(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TQueryResponse> Handle(TQueryRequest request, RequestHandlerDelegate<TQueryResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}