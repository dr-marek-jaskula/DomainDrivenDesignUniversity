using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;

namespace Shopway.Application.Mapping;

public static class ProductMapping
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        (
            Id: product.Id.Value,
            ProductName: product.ProductName.Value,
            Revision: product.Revision.Value,
            Price: product.Price.Value,
            UomCode: product.UomCode.Value,
            Reviews: product.Reviews.ToResponses()
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