using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Products.Queries;

public sealed record ProductResponse
(
    Guid Id,
    ProductName ProductName,
    Revision Revision,
    Price Price,
    UomCode UomCode,
    IReadOnlyCollection<Review> Reviews
)
    : IResponse;
