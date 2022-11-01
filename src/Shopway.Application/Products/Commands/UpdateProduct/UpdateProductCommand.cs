using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Commands.UpdateProduct;
public sealed record UpdateProductCommand
(
    Guid Id,
    string? ProductName,
    decimal? Price,
    string? UomCode,
    string? Revision
) : ICommand<Guid>;

