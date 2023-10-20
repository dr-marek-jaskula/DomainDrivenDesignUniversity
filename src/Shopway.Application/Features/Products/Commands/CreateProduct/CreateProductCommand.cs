using Shopway.Domain.EntityKeys;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand
(
    ProductKey ProductKey,
    decimal Price,
    string UomCode
) : ICommand<CreateProductResponse>;


