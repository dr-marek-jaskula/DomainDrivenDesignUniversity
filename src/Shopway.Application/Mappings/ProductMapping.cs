using Shopway.Application.CQRS;
using Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using System.Linq.Expressions;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.Mappings;

public static class ProductMapping
{
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

    /// <summary>
    /// Used for performance reasons. More info in: ReadMe.Application.md (Mapping section)
    /// </summary>
    /// <returns>An expression of func</returns>
    public static Expression<Func<Product, ProductResponse>> ToResponse()
    {
        return product => new ProductResponse
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
    }

    /// <summary>
    /// Used for performance reasons. More info in: ReadMe.Application.md (Mapping section). 
    /// </summary>
    /// <returns></returns>
    public static Expression<Func<Product, DictionaryResponseEntry>> ToDictionaryResponseEntry()
    {
        return product => new DictionaryResponseEntry
        (
            product.Id.Value,
            //Method *MapFromProductToProductKey* is not used on purpose here
            ProductKey.Create(product.ProductName.Value, product.Revision.Value)
        );
    }

    public static UpdateProductResponse ToUpdateResponse(this Product productToUpdate)
    {
        return new UpdateProductResponse(productToUpdate.Id.Value);
    }

    public static RemoveProductResponse ToRemoveResponse(this Product productToRemove)
    {
        return new RemoveProductResponse(productToRemove.Id.Value);
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
        return ProductKey.Create(productBatchRequest.ProductKey.ProductName, productBatchRequest.ProductKey.Revision);
    }

    public static ProductKey MapFromProductToProductKey(Product product)
    {
        return ProductKey.Create(product.ProductName.Value, product.Revision.Value);
    }
}