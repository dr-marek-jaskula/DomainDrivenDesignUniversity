using Shopway.Application.Features;
using Shopway.Application.Features.Orders.Queries;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using Shopway.Application.Features.Products.Commands.CreateProduct;
using Shopway.Application.Features.Products.Commands.RemoveProduct;
using Shopway.Application.Features.Products.Commands.UpdateProduct;
using Shopway.Application.Features.Products.Queries;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Domain.Products;
using System.Linq.Expressions;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Application.Mappings;

public static class ProductMapping
{
    /// <summary>
    /// Used for performance reasons. More info in: ReadMe.Application.md (Mapping section)
    /// </summary>
    /// <returns>An expression of func</returns>
    public static readonly Expression<Func<Product, ProductResponse>> ProductResponse = product => new ProductResponse
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
    public static readonly Expression<Func<Product, DictionaryResponseEntry<ProductKey>>> DictionaryResponseEntry = product => new DictionaryResponseEntry<ProductKey>
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

    public static ProductSummary ToSummary(this Product product)
    {
        return ProductSummary.Create
        (
            product.Id,
            product.ProductName.Value,
            product.Revision.Value,
            product.Price.Value,
            product.UomCode.Value
        ).Value;
    }

    public static ProductSummaryResponse ToSummaryResponse(this ProductSummary productSummary)
    {
        return new ProductSummaryResponse
        (
            productSummary.ProductId.Value,
            productSummary.ProductName.Value,
            productSummary.Revision.Value,
            productSummary.Price.Value,
            productSummary.UomCode.Value
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

    public static BatchUpsertProductResponse ToBatchProductUpsertResponse(this IList<BatchResponseEntry<ProductKey>> batchResponseEntries)
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
