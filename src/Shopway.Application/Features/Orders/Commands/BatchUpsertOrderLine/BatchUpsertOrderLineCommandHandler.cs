using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Microsoft.IdentityModel.Tokens;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using static Shopway.Application.Mappings.OrderLineMapping;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

public sealed partial class BatchUpsertOrderLineCommandHandler : IBatchCommandHandler<BatchUpsertOrderLineCommand, BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> _responseBuilder;

    public BatchUpsertOrderLineCommandHandler(IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> responseBuilder, IOrderHeaderRepository orderHeaderRepository, IProductRepository productRepository)
    {
        _responseBuilder = responseBuilder;
        _orderHeaderRepository = orderHeaderRepository;
        _productRepository = productRepository;
    }

    public async Task<IResult<BatchUpsertOrderLineResponse>> Handle(BatchUpsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<BatchUpsertOrderLineResponse>(Error.NullOrEmpty(nameof(BatchUpsertOrderLineCommand)));
        }

        var productIdsFromCommand = command.GetRequestsProductIds();
        var invalidProductIds = await _productRepository.VerifyIdsAsync(productIdsFromCommand, cancellationToken);

        if (invalidProductIds.NotNullOrEmpty())
        {
            return Result.Failure<BatchUpsertOrderLineResponse>(Error.InvalidReferences(invalidProductIds.GetUlids(), nameof(Product)));
        }

        var orderHeader = await _orderHeaderRepository.GetByIdAsync(command.OrderHeaderId, cancellationToken);

        var dictionaryOfOrderLinesToUpdate = GetDictionaryOfOrderLinesToUpdate(productIdsFromCommand, orderHeader);

        //Required step: set RequestToProductKeyMapping method for the injected builder
        _responseBuilder.SetRequestToResponseKeyMapper(MapFromRequestToOrderLineKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(_responseBuilder, dictionaryOfOrderLinesToUpdate);

        if (responseEntries.Any(response => response.Status is BatchEntryStatus.Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchInsertResponse());
        }

        InsertOrderLines(_responseBuilder.ValidRequestsToInsert, orderHeader);
        UpdateOrderLines(_responseBuilder.ValidRequestsToUpdate, dictionaryOfOrderLinesToUpdate);

        return responseEntries
            .ToBatchInsertResponse()
            .ToResult();
    }

    private static IDictionary<OrderLineKey, OrderLine> GetDictionaryOfOrderLinesToUpdate(IList<ProductId> productIds, OrderHeader orderHeader)
    {
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