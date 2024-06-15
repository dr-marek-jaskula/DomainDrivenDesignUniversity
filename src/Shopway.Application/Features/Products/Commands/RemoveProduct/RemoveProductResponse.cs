using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Commands.RemoveProduct;

public sealed record RemoveProductResponse
(
    Ulid Id
) : IResponse;
