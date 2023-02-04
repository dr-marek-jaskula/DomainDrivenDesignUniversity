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
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Application.Batch.BatchEntryStatus;
using static Shopway.Application.Mapping.ProductMapping;
using static Shopway.Application.Batch.Products.ProductBatchUpsertResponse;
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

        var productsToUpdateWithKeys = await GetProductsToUpdateWithKeys(command, cancellationToken);

        //Set proper Request to ProductKey mapping method for the injected builder
        _responseBuilder.SetRequestToResponseKeyMapper(MapFromRequestToResponseKey);

        //Perform validation, using the builder (with set RequestToResponse delegate), trimmed command and queried products
        var responseEntries = command.Validate(_responseBuilder, productsToUpdateWithKeys);

        if (responseEntries.Any(response => response.Status is Error))
        {
            return Result.BatchFailure(responseEntries.ToBatchUpsertResponse());
        }

        //If validation succeeded, then distinguish insert/update requests
        var validRequestsToInsert = _responseBuilder.ValidRequestsToInsert;
        var validRequestsToUpdate = _responseBuilder.ValidRequestsToUpdate;

        //Perform batch upsert
        await InsertProducts(validRequestsToInsert, cancellationToken);
        UpdateProducts(validRequestsToUpdate, productsToUpdateWithKeys);

        return responseEntries
            .ToBatchUpsertResponse()
            .ToResult();
    }

    private async Task<IDictionary<ProductKey, Product>> GetProductsToUpdateWithKeys(ProductBatchUpsertCommand command, CancellationToken cancellationToken)
    {
        var productNames = command.ProductNames();
        var productRevisions = command.ProductRevisions();

        var productsToBeFiltered = await _unitOfWork
            .Context
            .Set<Product>()
            .Where(product => productNames.Contains(product.ProductName.Value))
            .Where(product => productRevisions.Contains(product.Revision.Value))
            .OrderBy(product => product.ProductName.Value)
                .ThenBy(product => product.Revision.Value)
            .ToListAsync(cancellationToken);

        var sortedRequests = command
            .Requests
            .OrderBy(request => request.ProductName)
                .ThenBy(request => request.Revision)
            .ToList();

        return FilterSortedProducts(productsToBeFiltered, sortedRequests);
    }

    /// <summary>
    /// Both products and requests must be filtered in the same manner: using the product key order.
    /// Then the filtering will be efficient, because iterating if will just forward so no quadratic time will happen
    /// </summary>
    /// <param name="productsToBeFilteredSortedByKeyOrder">Products ordered by product key order</param>
    /// <param name="sortedRequestsByKeyOrder">Requests ordered by product key order</param>
    /// <returns>Products to be updated if further validation succeeds</returns>
    private static IDictionary<ProductKey, Product> FilterSortedProducts
    (
        IList<Product> productsToBeFilteredSortedByKeyOrder, 
        IList<ProductBatchUpsertRequest> sortedRequestsByKeyOrder
    )
    {
        int sortedRequestsIndex = 0;
        int productsToBefilteredIndex = 0;

        var products = new Dictionary<ProductKey, Product>();

        while (sortedRequestsIndex < sortedRequestsByKeyOrder.Count)
        {
            //Get the request and then check in order for matching product
            var request = sortedRequestsByKeyOrder[sortedRequestsIndex];

            while (productsToBefilteredIndex < productsToBeFilteredSortedByKeyOrder.Count)
            {
                var filtered = productsToBeFilteredSortedByKeyOrder[productsToBefilteredIndex];

                //If the product matches with the request, then the subsequent search will start from the next index
                productsToBefilteredIndex++;

                if (filtered.ProductName.Value == request.ProductName && filtered.Revision.Value == request.Revision)
                {
                    //If the product matches, get the key and add (key, product) to the dictionary
                    var key = MapFromProductToResponseKey(filtered);
                    products.Add(key, filtered);
                    break;
                }
            }

            //We move to the next request
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
            var key = MapFromRequestToResponseKey(request);
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