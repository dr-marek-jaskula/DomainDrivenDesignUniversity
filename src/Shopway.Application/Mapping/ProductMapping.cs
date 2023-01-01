using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Domain.Entities;

namespace Shopway.Application.Mapping;

public static class ProductMapping
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        (
            Id: product.Id.Value,
            ProductName: product.ProductName,
            Revision: product.Revision,
            Price: product.Price,
            UomCode: product.UomCode,
            Reviews: product.Reviews
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
}