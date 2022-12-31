using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.UpdateProduct;

public sealed record UpdateProductResponse
(
    Guid Id
) : IResponse;