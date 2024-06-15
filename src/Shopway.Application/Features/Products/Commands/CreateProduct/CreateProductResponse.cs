using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductResponse
(
    Ulid Id
) : IResponse;
