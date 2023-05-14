using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;

namespace Shopway.Application.CQRS.Orders.Queries;

public sealed record OrderHeaderResponse
(
    Guid Id,
    OrderStatus Status,
    Payment Payment,
    User User
) : IResponse;