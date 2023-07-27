using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Queries;

public sealed record ProductResponse
(
    Ulid Id,
    string ProductName,
    string Revision,
    decimal Price,
    string UomCode,
    IReadOnlyCollection<ReviewResponse> Reviews
)
    : IResponse;
