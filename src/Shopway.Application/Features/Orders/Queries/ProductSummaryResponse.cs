using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Queries;

public sealed record ProductSummaryResponse
(
    Ulid Id,
    string Name,
    string Revision,
    decimal Price,
    string UomCode
)
    : IResponse;
