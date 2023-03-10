using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityBusinessKeys;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

public sealed record CreateProductCommand
(
    ProductKey ProductKey,
    decimal Price,
    string UomCode
) : ICommand<CreateProductResponse>;


