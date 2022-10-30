namespace Shopway.Presentation.Requests.Products;

public sealed record CreateProductRequest
(
    string ProductName,
    int Price,
    string UomCode,
    string Revision
);