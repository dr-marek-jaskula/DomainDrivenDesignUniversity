using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Features.Orders.Queries;

public sealed record OrderHeaderResponse
(
    Ulid Id,
    string OrderStatus,
    string PaymentStatus,
    decimal TotalPayment,
    decimal TotalDiscount,
    IReadOnlyCollection<OrderLineResponse> OrderLines
)
    : IResponse, IHasCursor;