using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductResponse
(
    Ulid Id
) : IResponse;