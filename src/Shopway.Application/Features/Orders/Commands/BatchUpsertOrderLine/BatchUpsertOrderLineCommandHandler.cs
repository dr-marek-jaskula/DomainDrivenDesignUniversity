using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Domain.Products;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;
using static Shopway.Application.Mappings.OrderLineMapping;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

internal sealed partial class BatchUpsertOrderLineCommandHandler
(
    IBatchResponseBuilderFactory responseBuilderFactory,
    IOrderHeaderRepository orderHeaderRepository,
    IProductRepository productRepository
)
    : IBatchCommandHandler<BatchUpsertOrderLineCommand, BatchUpsertOrderLineRequest, BatchUpsertOrderLineResponse, OrderLineKey>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IBatchResponseBuilderFactory _responseBuilderFactory = responseBuilderFactory;
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    public async Task<IResult<BatchUpsertOrderLineResponse>> Handle(BatchUpsertOrderLineCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Error.NullOrEmpty(nameof(BatchUpsertOrderLineCommand))
                .ToResult<BatchUpsertOrderLineResponse>();
        }

        var productIdsFromCommand = command.GetRequestsProductIds();
        var products = await _productRepository.GetByIdsAsync(productIdsFromCommand, cancellationToken);

        var invalidProductIds = productIdsFromCommand
            .Except(products.Select(product => product.Id))
            .ToList();

        if (invalidProductIds.NotNullOrEmpty())
        {
            return Error.InvalidReferences(invalidProductIds.GetUlids(), nameof(Product))
                .ToResult<BatchUpsertOrderLineResponse>();
        }

        var orderHeader = await _orderHeaderRepository.GetByIdAsync(command.OrderHeaderId, cancellationToken);

        var dictionaryOfOrderLinesToUpdate = GetDictionaryOfOrderLinesToUpdate(products, orderHeader);

        var responseBuilder = _responseBuilderFactory.Create<BatchUpsertOrderLineRequest, OrderLineKey>(MapFromRequestToOrderLineKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(responseBuilder, dictionaryOfOrderLinesToUpdate);

        if (responseEntries.Any(response => response.Status is BatchEntryStatus.Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchOrderLineUpsertResponse());
        }

        var insertResult = InsertOrderLines(responseBuilder.ValidRequestsToInsert, orderHeader, products);

        if (insertResult.IsFailure)
        {
            return insertResult.Error
                .ToResult<BatchUpsertOrderLineResponse>();
        }

        UpdateOrderLines(responseBuilder.ValidRequestsToUpdate, dictionaryOfOrderLinesToUpdate);

        return responseEntries
            .ToBatchOrderLineUpsertResponse()
            .ToResult();
    }

    private static Dictionary<OrderLineKey, OrderLine> GetDictionaryOfOrderLinesToUpdate(IList<Product> products, OrderHeader orderHeader)
    {
        return orderHeader
            .OrderLines
            .Where(orderLine => products.Select(x => x.ToSummary()).Contains(orderLine.ProductSummary))
            .ToDictionary(orderLine => orderLine.ToOrderLineKey());
    }

    private static Result InsertOrderLines(IReadOnlyList<BatchUpsertOrderLineRequest> validRequestsToInsert, OrderHeader orderHeader, IList<Product> products)
    {
        foreach (var request in validRequestsToInsert)
        {
            var orderLineToInsert = OrderLine.Create
            (
                OrderLineId.New(),
                products.First(product => product.Id == request.ProductId).ToSummary(),
                orderHeader.Id,
                Amount.Create(request.Amount).Value,
                Discount.Create(request.Discount ?? 0).Value
            );

            var addOrderLineResult = orderHeader.AddOrderLine(orderLineToInsert);

            if (addOrderLineResult.IsFailure)
            {
                return addOrderLineResult;
            }
        }

        return Result.Success();
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
