using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Commands.UpdateProduct;
public sealed record UpdateProductCommand
(
    Guid Id,
    decimal Price
) : ICommand<Guid>;

