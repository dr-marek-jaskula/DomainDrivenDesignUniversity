using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Products.Queries.GetProductById;

public sealed record ProductResponse
(
    Guid Id,
    ProductName ProductName,
    Revision Revision,
    Price Price,
    UomCode UomCode,
    IReadOnlyCollection<Review> Reviews
);

