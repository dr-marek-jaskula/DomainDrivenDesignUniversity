using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products;
using static Shopway.Application.Features.Products.Commands.UpdateProduct.UpdateProductCommand;

namespace Shopway.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand
(
    ProductId Id,
    UpdateRequestBody Body

) : ICommand<UpdateProductResponse>
{
    public sealed record UpdateRequestBody(decimal Price);
}
