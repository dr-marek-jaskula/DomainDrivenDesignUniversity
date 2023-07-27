using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.RemoveProduct;

public sealed record RemoveProductResponse
(
    Ulid Id
) : IResponse;