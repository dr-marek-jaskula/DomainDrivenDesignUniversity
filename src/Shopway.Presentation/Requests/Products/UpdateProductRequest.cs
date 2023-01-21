using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Requests.Products;

public sealed record UpdateProductRequest
(
    decimal Price
) : IRequest;