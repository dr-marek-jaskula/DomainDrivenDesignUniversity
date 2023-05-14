using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Application.Abstractions.CQRS.Batch;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Application.Mappings.OrderLineMapping;
using static Shopway.Application.CQRS.BatchEntryStatus;
using static Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

public sealed partial class BatchUpsertOrderLineCommandHandler : IBatchCommandHandler<BatchUpsertOrderLineCommand, BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse>
{
    private readonly IUnitOfWork<ShopwayDbContext> _unitOfWork;
    private readonly IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> _responseBuilder;

    public BatchUpsertOrderLineCommandHandler
    (
        IUnitOfWork<ShopwayDbContext> unitOfWork,
        IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> responseBuilder
    )
    {
        _unitOfWork = unitOfWork;
        _responseBuilder = responseBuilder;
    }

    public async Task<IResult<BatchUpsertOrderLineResponse>> Handle(BatchUpsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<BatchUpsertOrderLineResponse>(NullOrEmpty(nameof(BatchUpsertOrderLineCommand)));
        }

        var orderLinesToUpdateDictionary = await GetOrderLinesToUpdateDictionary(command, cancellationToken);

        //Required step: set RequestToProductKeyMapping method for the injected builder
        _responseBuilder.SetRequestToResponseKeyMapper(MapFromRequestToOrderLineKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(_responseBuilder, orderLinesToUpdateDictionary);

        if (responseEntries.Any(response => response.Status is Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchInsertResponse());
        }

        await InsertOrderLines(command, _responseBuilder.ValidRequestsToInsert, cancellationToken);
        UpdateOrderLines(_responseBuilder.ValidRequestsToUpdate, orderLinesToUpdateDictionary);

        return responseEntries
            .ToBatchInsertResponse()
            .ToResult();
    }

    private async Task<IDictionary<OrderLineKey, OrderLine>> GetOrderLinesToUpdateDictionary(BatchUpsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        var productIds = command
            .Requests
            .Select(x => x.OrderLineKey.ProductId)
            .ToList();

        return await _unitOfWork
            .Context
            .Set<OrderLine>()
            .Where(orderLine => productIds.Contains(orderLine.ProductId))
            .ToDictionaryAsync(orderLine => orderLine.ToOrderLineKey(), cancellationToken);
    }

    private async Task InsertOrderLines(BatchUpsertOrderLineCommand command, IReadOnlyList<BatchUpsertOrderLineRequest> validRequestsToInsert, CancellationToken cancellationToken)
    {
        foreach (var request in validRequestsToInsert)
        {
            var orderLineToInsert = OrderLine.Create
            (
                OrderLineId.New(),
                request.OrderLineKey.ProductId,
                command.OrderHeaderId,
                Amount.Create(request.Amount).Value,
                Discount.Create(request.Discount ?? 0).Value
            );

            await _unitOfWork
                .Context
                .AddAsync(orderLineToInsert, cancellationToken);
        }
    }

    private static void UpdateOrderLines
    (
        IReadOnlyList<BatchUpsertOrderLineRequest> validRequestsToUpdate,
        IDictionary<OrderLineKey, OrderLine> orderLinesToUpdate
    )
    {
        foreach (var request in validRequestsToUpdate)
        {
            //At this stage, key is always valid
            var key = request.ToOrderLineKey();
            UpdateOrderLine(orderLinesToUpdate[key], request);
        }
    }

    private static void UpdateOrderLine(OrderLine orderLine, BatchUpsertOrderLineRequest request)
    {
        orderLine.UpdateAmount(Amount.Create(request.Amount).Value);
        orderLine.UpdateDiscount(Discount.Create(request.Discount ?? 0).Value);
    }
}