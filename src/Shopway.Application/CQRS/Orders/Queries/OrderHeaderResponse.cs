using Shopway.Application.Abstractions;
using Shopway.Domain.Enums;

namespace Shopway.Application.CQRS.Orders.Queries;

public sealed record OrderHeaderResponse
(
    Guid Id,
    OrderStatus OrderStatus,
    PaymentStatus PaymentStatus,
    decimal TotalPayment,
    decimal TotalDiscount,
    IReadOnlyCollection<OrderLineResponse> OrderLines

) : IResponse;