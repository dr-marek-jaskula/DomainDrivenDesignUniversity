﻿using MediatR;
using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Pipelines.CQRS;

public sealed class QueryTransactionPipeline<TQueryRequest, TQueryResponse>
    : QueryTransactionPipelineBase<TQueryResponse>, IPipelineBehavior<TQueryRequest, TQueryResponse>
    where TQueryRequest : class, IRequest<TQueryResponse>, IQuery<IResponse>
    where TQueryResponse : class, IResult<IResponse>
{
    public QueryTransactionPipeline(IUnitOfWork<ShopwayDbContext> unitOfWork)
        : base(unitOfWork)
    {
    }

    public async Task<TQueryResponse> Handle(TQueryRequest request, RequestHandlerDelegate<TQueryResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}