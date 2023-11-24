using Shopway.Domain.Orders;
using Shopway.Domain.Entities;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;

namespace Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine;

internal static class BatchUpsertOrderLineCommandValidator
{
    /// <summary>
    /// Validate the command
    /// </summary>
    /// <param name="command">Command to validate</param>
    /// <param name="responseBuilder">Response builder for given command</param>
    /// <param name="orderLinesToUpdateWithKeys">OrderLines meant to be updated with their unique keys</param>
    /// <returns>List of response entries, that are required to create the batch response</returns>
    public static IList<BatchResponseEntry> Validate
    (
        this BatchUpsertOrderLineCommand command,
        IBatchResponseBuilder<BatchUpsertOrderLineRequest, OrderLineKey> responseBuilder,
        IDictionary<OrderLineKey, OrderLine>  orderLinesToUpdateWithKeys
    )
    {
        var updateRequests = command.GetUpdateRequests(orderLinesToUpdateWithKeys);
        var insertRequests = command.GetInsertRequests(orderLinesToUpdateWithKeys);

        responseBuilder
            .ValidateUpdateRequests(updateRequests, ValidateRequest)
            .ValidateInsertRequests(insertRequests, ValidateRequest);

        return responseBuilder.BuildResponseEntries();
    }

    /// <summary>
    /// Governs the validation flow for the respective request
    /// </summary>
    /// <param name="responseEntryBuilder">Builder that corresponds to input request</param>
    /// <param name="request">Request to validate</param>
    private static void ValidateRequest
    (
        IBatchResponseEntryBuilder<BatchUpsertOrderLineRequest, OrderLineKey> responseEntryBuilder,
        BatchUpsertOrderLineRequest request
    )
    {
        responseEntryBuilder
            .UseValueObjectValidation<Amount>(request.Amount)
            .UseValueObjectValidation<Discount>(request.Discount ?? 0);
    }
}