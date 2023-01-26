using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand
(
    ProductId Id,
    decimal Price
) : ICommand<UpdateProductResponse>;

