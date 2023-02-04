using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Abstractions.Batch;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Application.Batch.Products.ProductBatchUpsertCommand;
using static Shopway.Application.Batch.Products.ProductBatchUpsertResponse;

namespace Shopway.Application.Batch.Products;

public static class ProductBatchUpserCommandValidator
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
        this ProductBatchUpsertCommand command, 
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
        //Due to the fact, that we want to return every possible error, we can not use the domain validation
        //Even if, the some validation logic is the same. 
        //Therefore, we are forced to duplicate a part of the validation 
        //However, we gain the generic validation for large batch operations

        //We can chain any number of validation methods, that can contain any number of validations
        responseEntryBuilder
            .ValidateUsing(ValidateProductName)
            .ValidateUsing(ValidateProductRevision)
            .ValidateUsing(ValidateProductPrice)
            .ValidateUsing(ValidateProductUomCode);
    }

    /// <summary>
    /// All validation logic for ProductName
    /// </summary>
    private static void ValidateProductName
    (
        IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> responseEntryBuilder, 
        ProductBatchUpsertRequest request
    )
    {
        responseEntryBuilder
            .If(request.ProductName.LengthNotInRange(1..ProductName.MaxLength), $"{nameof(ProductName)} must be in range '1..{ProductName.MaxLength}'. Current length: {request.ProductName.Length}")
            .If(request.ProductName.ContainsIllegalCharacter(), $"{ProductNameError.ContainsIllegalCharacter.Message}");
    }

    /// <summary>
    /// All validation logic for ProductRevision
    /// </summary>
    private static void ValidateProductRevision
    (
        IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> responseEntryBuilder, 
        ProductBatchUpsertRequest request
    )
    {
        responseEntryBuilder
            .If(request.Revision.LengthNotInRange(1..Revision.MaxLength), $"{nameof(Revision)} must be in range '1..{Revision.MaxLength}'. Current length: {request.Revision.Length}");
    }

    /// <summary>
    /// All validation logic for ProductPrice
    /// </summary>
    private static void ValidateProductPrice
    (
        IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> responseEntryBuilder, 
        ProductBatchUpsertRequest request
    )
    {
        responseEntryBuilder
            .If(request.Price.NotInRange(Price.MinPrice, Price.MaxPrice), $"{nameof(Price)} must be in range '{Price.MinPrice}..{Price.MaxPrice}'. Current length: {request.Price}");
    }

    /// <summary>
    /// All validation logic for ProductUomCode
    /// </summary>
    private static void ValidateProductUomCode
    (
        IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> responseEntryBuilder, 
        ProductBatchUpsertRequest request
    )
    {
        responseEntryBuilder
            .If(UomCode.AllowedUomCodes.NotContains(request.UomCode), $"{nameof(UomCode)} must be in '{UomCodeError.Invalid.Message}'. Current uom code: {request.UomCode}");
    }
}