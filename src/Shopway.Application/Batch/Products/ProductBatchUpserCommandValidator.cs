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
    public static IList<BatchResponseEntry> Validate
    (
        this ProductBatchUpsertCommand command, 
        IBatchResponseBuilder<ProductBatchUpsertRequest, ProductKey> builder, 
        IDictionary<ProductKey, Product> productsToUpdateWithKeys
    )
    {
        var updateRequests = command.GetUpdateRequests(productsToUpdateWithKeys);
        var insertRequests = command.GetInsertRequests(productsToUpdateWithKeys);

        builder
            .ValidateUpdateRequests(updateRequests, ValidateRequest)
            .ValidateInsertRequests(insertRequests, ValidateRequest);

        return builder.BuildResponseEntries();
    }

    private static void ValidateRequest(IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> builder, ProductBatchUpsertRequest request)
    {
        builder
            .ValidateUsing(ValidateProductName)
            .ValidateUsing(ValidateProductRevision)
            .ValidateUsing(ValidateProductPrice)
            .ValidateUsing(ValidateProductUomCode);
    }

    private static void ValidateProductName(IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> builder, ProductBatchUpsertRequest request)
    {
        builder
            .If(request.ProductName.LengthNotInRange(1..ProductName.MaxLength), $"{nameof(ProductName)} must be in range '1..{ProductName.MaxLength}'. Current length: {request.ProductName.Length}")
            .If(request.ProductName.ContainsIllegalCharacter(), $"{ProductNameError.ContainsIllegalCharacter.Message}");
    }

    private static void ValidateProductRevision(IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> builder, ProductBatchUpsertRequest request)
    {
        builder
            .If(request.Revision.LengthNotInRange(1..Revision.MaxLength), $"{nameof(Revision)} must be in range '1..{Revision.MaxLength}'. Current length: {request.Revision.Length}");
    }

    private static void ValidateProductPrice(IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> builder, ProductBatchUpsertRequest request)
    {
        builder
            .If(request.Price.NotInRange(Price.MinPrice, Price.MaxPrice), $"{nameof(Price)} must be in range '{Price.MinPrice}..{Price.MaxPrice}'. Current length: {request.Price}");
    }

    private static void ValidateProductUomCode(IBatchResponseEntryBuilder<ProductBatchUpsertRequest, ProductKey> builder, ProductBatchUpsertRequest request)
    {
        builder
            .If(UomCode.AllowedUomCodes.NotContains(request.UomCode), $"{nameof(UomCode)} must be in '{UomCodeError.Invalid.Message}'. Current uom code: {request.UomCode}");
    }
}