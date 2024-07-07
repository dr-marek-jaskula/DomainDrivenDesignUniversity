using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using ZiggyCreatures.Caching.Fusion;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;
using static Shopway.Application.Mappings.ProductMapping;
using static Shopway.Application.Utilities.CacheUtilities;

namespace Shopway.Application.Features.Products.Commands.BatchUpsertProduct;

public sealed partial class BatchUpsertProductCommandHandler
(
    IBatchResponseBuilderFactory responseBuilderFactory,
    IProductRepository productRepository,
    IFusionCache fusionCache
)
    : IBatchCommandHandler<BatchUpsertProductCommand, ProductBatchUpsertRequest, BatchUpsertProductResponse, ProductKey>
{
    private readonly IFusionCache _fusionCache = fusionCache;
    private readonly IBatchResponseBuilderFactory _responseBuilderFactory = responseBuilderFactory;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<BatchUpsertProductResponse>> Handle(BatchUpsertProductCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<BatchUpsertProductResponse>(Error.NullOrEmpty(nameof(BatchUpsertProductCommand)));
        }

        command = command.Trim();

        var productsToUpdateDictionary = await GetProductsToUpdateDictionary(command, cancellationToken);

        var responseBuilder = _responseBuilderFactory.Create<ProductBatchUpsertRequest, ProductKey>(MapFromRequestToProductKey);

        //Perform validation: using the builder, trimmed command and queried productsToUpdate
        var responseEntries = command.Validate(responseBuilder, productsToUpdateDictionary);

        if (responseEntries.Any(response => response.Status is BatchEntryStatus.Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchProductUpsertResponse());
        }

        //Perform batch upsert
        InsertProducts(responseBuilder.ValidRequestsToInsert);
        UpdateProducts(responseBuilder.ValidRequestsToUpdate, productsToUpdateDictionary);

        return responseEntries
            .ToBatchProductUpsertResponse()
            .ToResult();
    }

    private async Task<IDictionary<ProductKey, Product>> GetProductsToUpdateDictionary(BatchUpsertProductCommand command, CancellationToken cancellationToken)
    {
        var productNames = command.ProductNames();
        var productRevisions = command.ProductRevisions();

        return await _productRepository.GetProductsDictionaryByNameAndRevision
        (
            productNames,
            productRevisions,
            command.Requests.Select(x => x.ProductKey).ToList(),
            ProductMapping.ToProductKey,
            cancellationToken
        );
    }

    private void InsertProducts(IReadOnlyList<ProductBatchUpsertRequest> validRequestsToInsert)
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

            _productRepository.Create(productToInsert);
            _fusionCache.Set<Product, ProductId>(productToInsert);
        }
    }

    private void UpdateProducts
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

    private void UpdateProduct(Product product, ProductBatchUpsertRequest request)
    {
        product.UpdateUomCode(UomCode.Create(request.UomCode).Value);
        product.UpdatePrice(Price.Create(request.Price).Value);
        _fusionCache.Update<Product, ProductId>(product);
    }
}
