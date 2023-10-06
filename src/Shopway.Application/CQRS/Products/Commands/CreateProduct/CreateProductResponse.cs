using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

public sealed record CreateProductResponse
(
    Ulid Id
) : IResponse;