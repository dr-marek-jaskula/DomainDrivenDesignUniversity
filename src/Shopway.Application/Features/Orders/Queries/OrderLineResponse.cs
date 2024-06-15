using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Queries;

public sealed record OrderLineResponse
(
    Ulid Id,
    decimal Amount,
    decimal LineDiscount,
    decimal OrderLineCost,
    ProductSummaryResponse ProductSummary
)
    : IResponse;
