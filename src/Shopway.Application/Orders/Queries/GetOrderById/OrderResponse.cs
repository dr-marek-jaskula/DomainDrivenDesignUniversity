using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.Orders.Queries.GetOrderById;

public sealed record OrderResponse
(
    Guid Id,
    Amount Amount,
    Status Status,
    DateTimeOffset Deadline,
    Product Product,
    Payment Payment,
    Customer Customer
);