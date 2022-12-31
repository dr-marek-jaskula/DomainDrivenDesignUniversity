using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

public sealed record CreateProductCommand
(
    string ProductName,
    decimal Price,
    string UomCode,
    string Revision
) : ICommand<CreateProductResponse>;


