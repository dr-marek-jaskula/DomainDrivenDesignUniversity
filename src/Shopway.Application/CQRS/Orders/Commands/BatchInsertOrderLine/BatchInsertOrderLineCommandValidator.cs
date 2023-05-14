using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Application.Abstractions.CQRS.Batch;
using static Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine.BatchInsertOrderLineCommand;

namespace Shopway.Application.CQRS.Orders.Commands.BatchInsertOrderLine;

internal static class BatchInsertOrderLineCommandValidator
{
    /// <summary>
    /// Validate the command
    /// </summary>
    /// <param name="command">Command to validate</param>
    /// <param name="responseBuilder">Response builder for given command</param>
    /// <returns>List of response entries, that are required to create the batch response</returns>
    public static IList<BatchResponseEntry> Validate
    (
        this BatchInsertOrderLineCommand command,
        IBatchResponseBuilder<BatchInsertOrderLineRequest, OrderLineKey> responseBuilder
    )
    {
        responseBuilder
            .ValidateInsertRequests(command.Requests.AsReadOnly(), ValidateRequest);

        return responseBuilder.BuildResponseEntries();
    }

    /// <summary>
    /// Governs the validation flow for the respective request
    /// </summary>
    /// <param name="responseEntryBuilder">Builder that corresponds to input request</param>
    /// <param name="request">Request to validate</param>
    private static void ValidateRequest
    (
        IBatchResponseEntryBuilder<BatchInsertOrderLineRequest, OrderLineKey> responseEntryBuilder,
        BatchInsertOrderLineRequest request
    )
    {
        responseEntryBuilder
            .UseValueObjectValidation<Amount>(request.Amount)
            .UseValueObjectValidation<Discount>(request.Discount ?? 0);
    }
}