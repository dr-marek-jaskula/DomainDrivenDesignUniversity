using Shopway.Domain.Entities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Abstractions.CQRS.Batch;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.Features.Products.Commands.BatchUpsertProduct;

internal static class BatchUpsertProductCommandValidator
{
    /// <summary>
    /// Validate the command
    /// </summary>
    /// <param name="command">Command to validate</param>
    /// <param name="responseBuilder">Response builder for given command</param>
    /// <param name="productsToUpdateWithKeys">Product meant to be updated with their unique keys</param>
    /// <returns>List of response entries, that are required to create the batch response</returns>
    public static IList<BatchResponseEntry> Validate
    (
        this BatchUpsertProductCommand command,
        IBatchResponseBuilder<ProductBatchUpsertRequest, ProductKey> responseBuilder,
        IDictionary<ProductKey, Product> productsToUpdateWithKeys
    )
    {
        var updateRequests = command.GetUpdateRequests(productsToUpdateWithKeys);
        var insertRequests = command.GetInsertRequests(productsToUpdateWithKeys);

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
        IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> responseEntryBuilder,
        ProductBatchUpsertRequest request
    )
    {
        responseEntryBuilder
            .ValidateUsing(ValidateProductKey)
            .UseValueObjectValidation<Price>(request.Price)
            .UseValueObjectValidation<UomCode>(request.UomCode);
    }

    private static void ValidateProductKey
    (
        IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> responseEntryBuilder,
        ProductBatchUpsertRequest request
    )
    {
        responseEntryBuilder
            .UseValueObjectValidation<ProductName>(request.ProductKey.ProductName)
            .UseValueObjectValidation<Revision>(request.ProductKey.Revision);
    }
}