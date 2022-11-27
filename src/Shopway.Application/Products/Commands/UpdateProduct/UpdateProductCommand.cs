using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand
(
    ProductId Id,
    decimal Price
) : ICommand<Guid>;

