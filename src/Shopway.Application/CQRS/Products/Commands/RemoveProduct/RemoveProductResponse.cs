using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.RemoveProduct;

public sealed record RemoveProductResponse
(
    Guid Id
) : IResponse;