using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Application.CQRS.Orders.Queries.GetOrderById;

public sealed record OrderResponse
(
    Guid Id,
    Amount Amount,
    Status Status,
    Product Product,
    Payment Payment,
    Customer Customer
) : IResponse;