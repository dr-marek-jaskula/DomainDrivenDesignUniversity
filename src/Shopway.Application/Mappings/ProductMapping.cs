using Shopway.Application.CQRS;
using System.Linq.Expressions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.EntityKeys;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.Mappings;

public static class ProductMapping
{
    /// <summary>
    /// Used for performance reasons. More info in: ReadMe.Application.md (Mapping section)
    /// </summary>
    /// <returns>An expression of func</returns>
    public static Expression<Func<Product, ProductResponse>> ProductResponse => product => new ProductResponse
    (
        product.Id.Value,
        product.ProductName.Value,
        product.Revision.Value,
        product.Price.Value,
        product.UomCode.Value,
        product.Reviews
            .Select(review => new ReviewResponse
            (
                review.Id.Value,
                review.Username.Value,
                review.Stars.Value,
                review.Title.Value,
                review.Description.Value
            ))
            .ToList()
            .AsReadOnly()
    );

    /// <summary>
    /// Used for performance reasons. More info in: ReadMe.Application.md (Mapping section). 
    /// </summary>
    /// <returns></returns>
    public static Expression<Func<Product, DictionaryResponseEntry>> DictionaryResponseEntry => product => new DictionaryResponseEntry
    (
        product.Id.Value,
        //Method *ToProductKey* is not used on purpose here
        ProductKey.Create(product.ProductName.Value, product.Revision.Value)
    );

    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        (
            product.Id.Value,
            product.ProductName.Value,
            product.Revision.Value,
            product.Price.Value,
            product.UomCode.Value,
            product.Reviews.ToResponses()
        );
    }

    public static UpdateProductResponse ToUpdateResponse(this Product productToUpdate)
    {
        return new UpdateProductResponse(productToUpdate.Id.Value);
    }

    public static RemoveProductResponse ToRemoveResponse(this ProductId productIdToRemove)
    {
        return new RemoveProductResponse(productIdToRemove.Value);
    }

    public static CreateProductResponse ToCreateResponse(this Product productToCreate)
    {
        return new CreateProductResponse(productToCreate.Id.Value);
    }

    public static BatchUpsertProductResponse ToBatchUpsertResponse(this IList<BatchResponseEntry> batchResponseEntries)
    {
        return new BatchUpsertProductResponse(batchResponseEntries);
    }

    public static ProductKey MapFromRequestToProductKey(ProductBatchUpsertRequest productBatchRequest)
    {
        return productBatchRequest.ProductKey;
    }

    public static ProductKey ToProductKey(this ProductBatchUpsertRequest productBatchRequest)
    {
        return productBatchRequest.ProductKey;
    }

    public static ProductKey ToProductKey(this Product product)
    {
        return ProductKey.Create(product.ProductName.Value, product.Revision.Value);
    }
}