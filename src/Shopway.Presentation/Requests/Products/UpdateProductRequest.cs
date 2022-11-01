namespace Shopway.Presentation.Requests.Products;

public sealed record UpdateProductRequest
(
    string? ProductName,
    int? Price,
    string? UomCode,
    string? Revision
);