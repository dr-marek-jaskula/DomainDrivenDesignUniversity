using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Orders;

public sealed record OrderLineResponse
(
    Guid Id,
    decimal Amount,
    decimal LineDiscount,
    string ProductName,
    string Revision,
    decimal ProductPrice,
    decimal OrderLineCost
) : IResponse;