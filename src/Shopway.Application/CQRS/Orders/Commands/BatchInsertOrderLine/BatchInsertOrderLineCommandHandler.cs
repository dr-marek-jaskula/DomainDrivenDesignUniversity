using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Application.Abstractions.CQRS.Batch;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Application.Mappings.OrderLineMapping;
using static Shopway.Application.CQRS.BatchEntryStatus;
using static Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine.BatchInsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine;

public sealed partial class BatchInsertOrderLineCommandHandler : IBatchCommandHandler<BatchInsertOrderLineCommand, BatchInsertOrderLineRequest, BatchInsertOrderLineResponse>
{
    private readonly IUnitOfWork<ShopwayDbContext> _unitOfWork;
    private readonly IBatchResponseBuilder<BatchInsertOrderLineRequest, OrderLineKey> _responseBuilder;

    public BatchInsertOrderLineCommandHandler
    (
        IUnitOfWork<ShopwayDbContext> unitOfWork,
        IBatchResponseBuilder<BatchInsertOrderLineRequest, OrderLineKey> responseBuilder
    )
    {
        _unitOfWork = unitOfWork;
        _responseBuilder = responseBuilder;
    }

    public async Task<IResult<BatchInsertOrderLineResponse>> Handle(BatchInsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<BatchInsertOrderLineResponse>(NullOrEmpty(nameof(BatchInsertOrderLineCommand)));
        }

        //Required step: set RequestToProductKeyMapping method for the injected builder
        _responseBuilder.SetRequestToResponseKeyMapper(MapFromRequestToOrderLineKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(_responseBuilder);

        if (responseEntries.Any(response => response.Status is Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchInsertResponse());
        }

        await InsertOrderLines(command, _responseBuilder.ValidRequestsToInsert, cancellationToken);

        return responseEntries
            .ToBatchInsertResponse()
            .ToResult();
    }

    private async Task InsertOrderLines(BatchInsertOrderLineCommand command, IReadOnlyList<BatchInsertOrderLineRequest> validRequestsToInsert, CancellationToken cancellationToken)
    {
        foreach (var request in validRequestsToInsert)
        {
            var orderLineToInsert = OrderLine.Create
            (
                OrderLineId.New(),
                request.ProductId,
                command.OrderHeaderId,
                Amount.Create(request.Amount).Value,
                Discount.Create(request.Discount ?? 0).Value
            );

            await _unitOfWork
                .Context
                .AddAsync(orderLineToInsert, cancellationToken);
        }
    }
}