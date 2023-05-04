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
using static Shopway.Application.CQRS.BatchEntryStatus;
using static Shopway.Application.Mappings.ProductMapping;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;
using System.Collections.Immutable;

namespace Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;

public sealed partial class BatchUpsertProductCommandHandler : IBatchCommandHandler<BatchUpsertProductCommand, ProductBatchUpsertRequest, BatchUpsertProductResponse>
{
    private readonly IUnitOfWork<ShopwayDbContext> _unitOfWork;
    private readonly IBatchResponseBuilder<ProductBatchUpsertRequest, ProductKey> _responseBuilder;

    public BatchUpsertProductCommandHandler
    (
        IUnitOfWork<ShopwayDbContext> unitOfWork,
        IBatchResponseBuilder<ProductBatchUpsertRequest, ProductKey> responseBuilder
    )
    {
        _unitOfWork = unitOfWork;
        _responseBuilder = responseBuilder;
    }

    public async Task<IResult<BatchUpsertProductResponse>> Handle(BatchUpsertProductCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<BatchUpsertProductResponse>(NullOrEmpty(nameof(BatchUpsertProductCommand)));
        }

        command = command.Trim();

        var productsToUpdateDictionary = await GetProductsToUpdateDictionary(command, cancellationToken);

        //Required step: set RequestToProductKeyMapping method for the injected builder
        _responseBuilder.SetRequestToResponseKeyMapper(MapFromRequestToProductKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(_responseBuilder, productsToUpdateDictionary);

        if (responseEntries.Any(response => response.Status is Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchUpsertResponse());
        }

        //Perform batch upsert
        await InsertProducts(_responseBuilder.ValidRequestsToInsert, cancellationToken);
        UpdateProducts(_responseBuilder.ValidRequestsToUpdate, productsToUpdateDictionary);

        return responseEntries
            .ToBatchUpsertResponse()
            .ToResult();
    }

    private async Task<IDictionary<ProductKey, Product>> GetProductsToUpdateDictionary(BatchUpsertProductCommand command, CancellationToken cancellationToken)
    {
        var productNames = command.ProductNames();
        var productRevisions = command.ProductRevisions();

        //We query too many products, because we query all combinations of ProductName and Revision. Therefore, we will need to filter them
        var productsToBeFiltered = await _unitOfWork
            .Context
            .Set<Product>()
            .Where(product => productNames.Contains(product.ProductName.Value))
            .Where(product => productRevisions.Contains(product.Revision.Value))
            .ToListAsync(cancellationToken);

        return productsToBeFiltered
            .Where(product => command.Requests.Any(request => request.ProductKey.ProductName.CaseInsensitiveEquals(product.ProductName.Value) 
                                                           && request.ProductKey.Revision.CaseInsensitiveEquals(product.Revision.Value)))
            .ToDictionary(product => product.ToProductKey());
    }

    private async Task InsertProducts(IReadOnlyList<ProductBatchUpsertRequest> validRequestsToInsert, CancellationToken cancellationToken)
    {
        foreach (var request in validRequestsToInsert)
        {
            var productToInsert = Product.Create
            (
                ProductId.New(),
                ProductName.Create(request.ProductKey.ProductName).Value,
                Price.Create(request.Price).Value,
                UomCode.Create(request.UomCode).Value,
                Revision.Create(request.ProductKey.Revision).Value
            );

            await _unitOfWork
                .Context
                .AddAsync(productToInsert, cancellationToken);
        }
    }

    private static void UpdateProducts
    (
        IReadOnlyList<ProductBatchUpsertRequest> validRequestsToUpdate,
        IDictionary<ProductKey, Product> productsToUpdate
    )
    {
        foreach (var request in validRequestsToUpdate)
        {
            //At this stage, key is always valid
            var key = request.ToProductKey();
            UpdateProduct(productsToUpdate[key], request);
        }
    }

    private static void UpdateProduct(Product product, ProductBatchUpsertRequest request)
    {
        product.UpdateName(ProductName.Create(request.ProductKey.ProductName).Value);
        product.UpdateRevision(Revision.Create(request.ProductKey.Revision).Value);
        product.UpdateUomCode(UomCode.Create(request.UomCode).Value);
        product.UpdatePrice(Price.Create(request.Price).Value);
    }
}