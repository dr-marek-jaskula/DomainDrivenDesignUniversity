using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions.Batch;
using Shopway.Application.Mapping;
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
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Application.Batch.BatchEntryStatus;
using static Shopway.Application.Mapping.ProductMapping;
using static Shopway.Application.Batch.Products.ProductBatchUpsertCommand;

namespace Shopway.Application.Batch.Products;

public sealed partial class ProductBatchUpsertCommandHandler : IBatchCommandHandler<ProductBatchUpsertCommand, ProductBatchUpsertRequest, ProductBatchUpsertResponse>
{
    private readonly IUnitOfWork<ShopwayDbContext> _unitOfWork;
    private readonly IBatchResponseBuilder<ProductBatchUpsertRequest, ProductKey> _responseBuilder;

    public ProductBatchUpsertCommandHandler
    (
        IUnitOfWork<ShopwayDbContext> unitOfWork, 
        IBatchResponseBuilder<ProductBatchUpsertRequest, ProductKey> responseBuilder
    )
    {
        _unitOfWork = unitOfWork;
        _responseBuilder = responseBuilder;
    }

    public async Task<IResult<ProductBatchUpsertResponse>> Handle(ProductBatchUpsertCommand command, CancellationToken cancellationToken)
    {
        if (command.Requests.IsNullOrEmpty())
        {
            return Result.Failure<ProductBatchUpsertResponse>(NullOrEmpty(nameof(ProductBatchUpsertCommand)));
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

        //If validation succeeded, then distinguish insert/update requests
        var validRequestsToInsert = _responseBuilder.ValidRequestsToInsert;
        var validRequestsToUpdate = _responseBuilder.ValidRequestsToUpdate;

        //Perform batch upsert
        await InsertProducts(validRequestsToInsert, cancellationToken);
        UpdateProducts(validRequestsToUpdate, productsToUpdateDictionary);

        return responseEntries
            .ToBatchUpsertResponse()
            .ToResult();
    }

    private async Task<IDictionary<ProductKey, Product>> GetProductsToUpdateDictionary(ProductBatchUpsertCommand command, CancellationToken cancellationToken)
    {
        var productNames = command.ProductNames();
        var productRevisions = command.ProductRevisions();

        //We query too many products, because we query all combinations of ProductName and Revision
        //Therefore, we will need to filter them
        var productsToBeFiltered = await _unitOfWork
            .Context
            .Set<Product>()
            .Where(product => productNames.Contains(product.ProductName.Value))
            .Where(product => productRevisions.Contains(product.Revision.Value))
            .OrderBy(product => product.ProductName.Value)
                .ThenBy(product => product.Revision.Value)
            .ToListAsync(cancellationToken);

        var requestsSortedInTheSameMannerAsQueriedProducts = command
            .Requests
            .OrderBy(request => request.ProductName)
                .ThenBy(request => request.Revision)
            .ToList();

        return FilterProductsAndMapToDictionary(productsToBeFiltered, requestsSortedInTheSameMannerAsQueriedProducts);
    }

    /// <summary>
    /// Both products and requests must be filtered in the same manner: using the product key order.
    /// Then, filtering will be efficient, because iterating will just go forward
    /// </summary>
    /// <param name="productsToBeFilteredSortedByKey">Products ordered by product key order</param>
    /// <param name="sortedRequestsByKeyOrder">Requests ordered by product key order</param>
    /// <returns>Products to be updated</returns>
    private static IDictionary<ProductKey, Product> FilterProductsAndMapToDictionary
    (
        IList<Product> productsToBeFilteredSortedByKey, 
        IList<ProductBatchUpsertRequest> sortedRequestsByKeyOrder
    )
    {
        int sortedRequestsIndex = 0;
        int productsToBefilteredIndex = 0;

        var products = new Dictionary<ProductKey, Product>();

        while (sortedRequestsIndex < sortedRequestsByKeyOrder.Count)
        {
            //Get a request and then search for the matching product
            var request = sortedRequestsByKeyOrder[sortedRequestsIndex];

            while (productsToBefilteredIndex < productsToBeFilteredSortedByKey.Count)
            {
                var filtered = productsToBeFilteredSortedByKey[productsToBefilteredIndex];

                //If the product matches a request, then the subsequent search will start from the next index
                productsToBefilteredIndex++;

                if (filtered.ProductName.Value.CaseInsensitiveEquals(request.ProductName) && filtered.Revision.Value.CaseInsensitiveEquals(request.Revision))
                {
                    //If the product matches, first get the key and then add (key, product) to the dictionary
                    var key = MapFromProductToProductKey(filtered);
                    products.Add(key, filtered);
                    break;
                }
            }

            //Move to the next request
            sortedRequestsIndex++;
        }

        return products;
    }

    private async Task InsertProducts(IReadOnlyList<ProductBatchUpsertRequest> validRequestsToInsert, CancellationToken cancellationToken)
    {
        foreach (var request in validRequestsToInsert)
        {
            var productToInsert = Product.Create
            (
                ProductId.New(),
                ProductName.Create(request.ProductName).Value,
                Price.Create(request.Price).Value,
                UomCode.Create(request.UomCode).Value,
                Revision.Create(request.Revision).Value
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
            var key = MapFromRequestToProductKey(request);
            UpdateProduct(productsToUpdate[key], request);
        }
    }

    private static void UpdateProduct(Product product, ProductBatchUpsertRequest request)
    {
        product.UpdateName(ProductName.Create(request.ProductName).Value);
        product.UpdateRevision(Revision.Create(request.Revision).Value);
        product.UpdateUomCode(UomCode.Create(request.UomCode).Value);
        product.UpdatePrice(Price.Create(request.Price).Value);
    }
}