using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Products.Commands.UpdateProduct.UpdateProductCommand;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand
(
    ProductId Id,
    UpdateRequestBody Body

) : ICommand<UpdateProductResponse>
{
    public sealed record UpdateRequestBody(decimal Price);
}