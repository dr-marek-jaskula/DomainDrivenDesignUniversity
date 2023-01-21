using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Requests.Products;

public sealed record CreateProductRequest
(
    string ProductName,
    decimal Price,
    string UomCode,
    string Revision
) : IRequest;