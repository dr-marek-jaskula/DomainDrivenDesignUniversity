﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Pipelines;

public sealed class CommandWithResponseTransactionPipeline<TCommandRequest, TCommandResponse>(IUnitOfWork<ShopwayDbContext> unitOfWork)
    : CommandTransactionPipelineBase<TCommandResponse>(unitOfWork),
      IPipelineBehavior<TCommandRequest, TCommandResponse>
        where TCommandRequest : class, IRequest<TCommandResponse>, ICommand<IResponse>
        where TCommandResponse : class, IResult<IResponse>
{
    public async Task<TCommandResponse> Handle(TCommandRequest command, RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        var executionStrategy = UnitOfWork.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(cancellationToken => BeginTransactionAsync(next, cancellationToken), cancellationToken);
    }
}