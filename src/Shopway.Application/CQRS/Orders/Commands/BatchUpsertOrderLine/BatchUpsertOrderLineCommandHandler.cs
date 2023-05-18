using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityKeys;
using ZiggyCreatures.Caching.Fusion;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Abstractions.Repositories;
using static Shopway.Persistence.Utilities.CacheUtilities;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Application.Mappings.OrderLineMapping;
using static Shopway.Application.CQRS.BatchEntryStatus;
using static Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchUpsertOrderLine;

public sealed partial class BatchUpsertOrderLineCommandHandler : IBatchCommandHandler<BatchUpsertOrderLineCommand, BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly IFusionCache _fusionCache;
    private readonly IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> _responseBuilder;

    public BatchUpsertOrderLineCommandHandler(IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> responseBuilder, IOrderHeaderRepository orderHeaderRepository, IFusionCache fusionCache)
    {
        _responseBuilder = responseBuilder;
        _orderHeaderRepository = orderHeaderRepository;
        _fusionCache = fusionCache;
    }

    public async Task<IResult<BatchUpsertOrderLineResponse>> Handle(BatchUpsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<BatchUpsertOrderLineResponse>(NullOrEmpty(nameof(BatchUpsertOrderLineCommand)));
        }

        var orderHeader = await _orderHeaderRepository.GetByIdAsync(command.OrderHeaderId, cancellationToken);

        var orderLinesToUpdateDictionary = GetOrderLinesToUpdateDictionary(command, orderHeader, cancellationToken);

        //Required step: set RequestToProductKeyMapping method for the injected builder
        _responseBuilder.SetRequestToResponseKeyMapper(MapFromRequestToOrderLineKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(_responseBuilder, orderLinesToUpdateDictionary);

        if (responseEntries.Any(response => response.Status is Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchInsertResponse());
        }

        InsertOrderLines(_responseBuilder.ValidRequestsToInsert, orderHeader);
        UpdateOrderLines(_responseBuilder.ValidRequestsToUpdate, orderLinesToUpdateDictionary);

        _fusionCache.Update<OrderHeader, OrderHeaderId>(orderHeader);

        return responseEntries
            .ToBatchInsertResponse()
            .ToResult();
    }

    private static IDictionary<OrderLineKey, OrderLine> GetOrderLinesToUpdateDictionary(BatchUpsertOrderLineCommand command, OrderHeader orderHeader, CancellationToken cancellationToken)
    {
        var productIds = command
            .Requests
            .Select(x => x.ProductId)
            .ToList();

        return orderHeader
            .OrderLines
            .Where(orderLine => productIds.Contains(orderLine.ProductId))
            .ToDictionary(orderLine => orderLine.ToOrderLineKey());
    }

    private static void InsertOrderLines(IReadOnlyList<BatchUpsertOrderLineRequest> validRequestsToInsert, OrderHeader orderHeader)
    {
        foreach (var request in validRequestsToInsert)
        {
            var orderLineToInsert = OrderLine.Create
            (
                OrderLineId.New(),
                request.ProductId,
                orderHeader.Id,
                Amount.Create(request.Amount).Value,
                Discount.Create(request.Discount ?? 0).Value
            );

            orderHeader.AddOrderLine(orderLineToInsert);
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